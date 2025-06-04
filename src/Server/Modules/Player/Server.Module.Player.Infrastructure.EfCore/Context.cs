using Microsoft.EntityFrameworkCore;

namespace Server.Module.Player.Infrastructure.EfCore;

public class Context : DbContext
{
    public DbSet<PlayerEntity> PlayerEntities { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Context"/> и гарантирует создание базы данных.
    /// </summary>
    /// <param name="options">Параметры, используемые для конфигурации DbContext.</param>
    public Context(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    /// <summary>
    /// Настраивает сопоставление сущностей для контекста базы данных с использованием предоставленного построителя модели.
    /// </summary>
    /// <param name="modelBuilder">Построитель, используемый для создания модели для контекста.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PlayerConfiguration());
    }
}
