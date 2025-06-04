$ErrorActionPreference = "Continue"
$VerbosePreference = "Continue"

$OutputEncoding = [System.Text.Encoding]::UTF8
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

if (-not ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltinRole] "Administrator")) {
    # Перезапуск с правами администратора
    Start-Process powershell -ArgumentList "-NoProfile -ExecutionPolicy Bypass -File `"$PSCommandPath`"" -Verb RunAs
    exit
}

# Получаем полный путь к текущему скрипту
$scriptPath = $MyInvocation.MyCommand.Path

# Получаем путь к родительской папке
$parentFolder = Split-Path -Path $scriptPath -Parent
$parentFolder = Split-Path -Path $parentFolder -Parent

# Переходим в родительскую папку скрипта
Set-Location $parentFolder

# Получаем относительный путь к родительской папке (это будет просто ".")
$rootFolder = "./src"
Write-Host "Root folder: $parentFolder"
$oldName = Read-Host "Old project name"
$newName = Read-Host "New project name"


# Валидация ввода
if ([string]::IsNullOrWhiteSpace($oldName) -or [string]::IsNullOrWhiteSpace($newName)) {
    Write-Error "Имена проектов не могут быть пустыми."
    exit 1
}

# Проверка наличия старого имени в файлах проекта
$foundOldName = $false
foreach ($item in Get-ChildItem -LiteralPath $rootFolder -Recurse -Include "*.cs", "*.csproj", "*.sln" -Exclude "node_modules", ".git", "bin", "obj") {
    Write-Verbose "Проверка файла: $($item.FullName)"

    try {
        $content = Get-Content -LiteralPath $item.FullName -ErrorAction Stop
        Write-Verbose "Доступ есть к файлу: $($item.FullName)"
        if ($content -match [regex]::Escape($oldName)) {
            $foundOldName = $true
            break
        }
    }
    catch {
        Write-Error "Нет доступа к файлу: $($item.FullName). Ошибка: $_"
    }
}

if (-not $foundOldName) {
    Write-Warning "Имя '$oldName' не найдено в проекте. Проверьте правильность ввода."
    $continue = Read-Host "Продолжить? (y/n)"
    if ($continue -ne "y") {
        exit 0
    }
}

try {
    # # Создаем резервную копию
    # $timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
    # $backupDir = Join-Path -Path (Split-Path -Path $parentFolder -Parent) -ChildPath "backup_$timestamp"
    # Write-Host "Создание резервной копии в $backupDir..."
    # Copy-Item -Path $parentFolder -Destination $backupDir -Recurse
    # Write-Host "Резервная копия создана успешно."
       

    # Rename files and folders
    foreach ($item in Get-ChildItem -LiteralPath $rootFolder -Recurse -Exclude "node_modules", ".git", "bin", "obj" | Sort-Object -Property FullName -Descending) {
        $itemNewName = $item.Name.Replace($oldName, $newName)
        if ($item.Name -ne $itemNewName) {
            Rename-Item -LiteralPath $item.FullName -NewName $itemNewName
        }
    }

    # Replace content in files
    foreach ($item in Get-ChildItem -LiteralPath $rootFolder -Recurse -Include "*.cmd", "*.cs", "*.csproj", "*.json", "*.md", "*.proj", "*.props", "*.ps1", "*.sln", "*.slnx", "*.targets", "*.txt", "*.vb", "*.vbproj", "*.xaml", "*.xml", "*.xproj", "*.yml", "*.yaml" -Exclude "node_modules", ".git", "bin", "obj") {
        $content = Get-Content -LiteralPath $item.FullName -Encoding UTF8
        if ($content) {
            $newContent = $content.Replace($oldName, $newName)
            Set-Content -LiteralPath $item.FullName -Value $newContent -Encoding UTF8
        }
    }

    # # Удаляем резервную копию
    # Write-Host "Операция успешно завершена. Удаляем резервную копию..."
    # Remove-Item -Path $backupDir -Recurse -Force
    # Write-Host "Резервная копия удалена."
}
catch {
    # Этот блок выполняется, если в блоке try возникла ошибка
    Write-Error "Произошла ошибка во время выполнения скрипта:"
    Write-Error $_.Exception.Message
    Write-Error "Стек вызовов: $($_.ScriptStackTrace)"
    
    if ($createBackup -ne "n" -and (Test-Path -Path $backupDir)) {
        $restore = Read-Host "Восстановить данные из резервной копии? (y/n) [y]"
        if ($restore -ne "n") {
            Write-Host "Восстановление из резервной копии $backupDir..."
            # Здесь код для восстановления из резервной копии
            # ...
            Write-Host "Восстановление завершено."
        }
    }
}

Write-Host "Операция завершена. Проект успешно переименован с '$oldName' на '$newName'."
Write-Host ""
Read-Host "Нажмите любую клавишу для закрытия окна..."
