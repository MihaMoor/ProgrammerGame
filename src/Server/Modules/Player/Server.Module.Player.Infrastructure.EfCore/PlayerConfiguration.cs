using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server.Module.Player.Infrastructure.EfCore;

internal class PlayerConfiguration : IEntityTypeConfiguration<PlayerEntity>
{
    /// <summary>
    /// Configures the entity mapping for <see cref="PlayerEntity"/>, setting the primary key and defining indexes on relevant properties.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="PlayerEntity"/> entity type.</param>
    public void Configure(EntityTypeBuilder<PlayerEntity> builder)
    {
        builder.HasKey(x => x.PlayerId);
        builder.HasIndex(x => x.PlayerId);
        builder.HasIndex(x => x.IsAlive);
    }
}
