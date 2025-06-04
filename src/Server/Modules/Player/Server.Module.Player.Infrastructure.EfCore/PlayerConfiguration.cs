using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server.Module.Player.Infrastructure.EfCore;

internal class PlayerConfiguration : IEntityTypeConfiguration<PlayerEntity>
{
    /// <summary>
    /// Настраивает отображение сущности <see cref="PlayerEntity"/>, устанавливая первичный ключ и определяя индексы по PlayerId и IsAlive.
    /// </summary>
    /// <param name="builder">Построитель, используемый для конфигурации типа PlayerEntity.</param>
    public void Configure(EntityTypeBuilder<PlayerEntity> builder)
    {
        builder.HasKey(x => x.PlayerId);
        builder.HasIndex(x => x.PlayerId);
        builder.HasIndex(x => x.IsAlive);
    }
}
