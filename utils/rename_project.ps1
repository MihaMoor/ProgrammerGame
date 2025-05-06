$ErrorActionPreference = "Stop"

$OutputEncoding = [System.Text.Encoding]::UTF8
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

# Получаем полный путь к текущему скрипту
$scriptPath = $MyInvocation.MyCommand.Path

# Получаем путь к родительской папке
$parentFolder = Split-Path -Path $scriptPath -Parent
$parentFolder = Split-Path -Path $parentFolder -Parent

# Переходим в родительскую папку скрипта
Set-Location $parentFolder

# Получаем относительный путь к родительской папке (это будет просто ".")
$rootFolder = "."
Write-Host "Root folder: $parentFolder"
$oldName = Read-Host "Old project name"
$newName = Read-Host "New project name"

try {
    # Rename files and folders
    foreach ($item in Get-ChildItem -LiteralPath $rootFolder -Recurse | Sort-Object -Property FullName -Descending) {
        $itemNewName = $item.Name.Replace($oldName, $newName)
        if ($item.Name -ne $itemNewName) {
            Rename-Item -LiteralPath $item.FullName -NewName $itemNewName
        }
    }

    # Replace content in files
    foreach ($item in Get-ChildItem -LiteralPath $rootFolder -Recurse -Include "*.cmd", "*.cs", "*.csproj", "*.json", "*.md", "*.proj", "*.props", "*.ps1", "*.sln", "*.slnx", "*.targets", "*.txt", "*.vb", "*.vbproj", "*.xaml", "*.xml", "*.xproj", "*.yml", "*.yaml") {
        $content = Get-Content -LiteralPath $item.FullName
        if ($content) {
            $newContent = $content.Replace($oldName, $newName)
            Set-Content -LiteralPath $item.FullName -Value $newContent
        }
    }
}
catch {
    # Этот блок выполняется, если в блоке try возникла ошибка
    Write-Error "Error:"
    Write-Error $_.Exception.Message
}

Read-Host "Press any key to close window..."