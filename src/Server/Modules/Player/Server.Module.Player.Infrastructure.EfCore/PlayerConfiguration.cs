using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server.Module.Player.Infrastructure.EfCore;

internal class PlayerConfiguration : IEntityTypeConfiguration<PlayerEntity>
{
    /// <summary>
    /// ����������� ����������� �������� <see cref="PlayerEntity"/>, ������������ ��������� ���� � ��������� ������� �� PlayerId � IsAlive.
    /// </summary>
    /// <param name="builder">�����������, ������������ ��� ������������ ���� PlayerEntity.</param>
    public void Configure(EntityTypeBuilder<PlayerEntity> builder)
    {
        builder.HasKey(x => x.PlayerId);
        builder.HasIndex(x => x.PlayerId);
        builder.HasIndex(x => x.IsAlive);
    }
}
