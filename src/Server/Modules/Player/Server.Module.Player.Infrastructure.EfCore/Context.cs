using Microsoft.EntityFrameworkCore;

namespace Server.Module.Player.Infrastructure.EfCore;

public class Context : DbContext
{
    public DbSet<PlayerEntity> PlayerEntities { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Context"/> class and ensures the database is created.
    /// </summary>
    public Context(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    /// <summary>
    /// Configures the entity model for the context using the specified model builder.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for the context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PlayerConfiguration());
    }
}
