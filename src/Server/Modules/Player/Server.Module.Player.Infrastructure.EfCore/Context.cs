using Microsoft.EntityFrameworkCore;

namespace Server.Module.Player.Infrastructure.EfCore;

public class Context : DbContext
{
    public DbSet<PlayerEntity> PlayerEntities { get; set; }

    /// <summary>
    /// �������������� ����� ��������� ������ <see cref="Context"/> � ����������� �������� ���� ������.
    /// </summary>
    /// <param name="options">���������, ������������ ��� ������������ DbContext.</param>
    public Context(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    /// <summary>
    /// ����������� ������������� ��������� ��� ��������� ���� ������ � �������������� ���������������� ����������� ������.
    /// </summary>
    /// <param name="modelBuilder">�����������, ������������ ��� �������� ������ ��� ���������.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PlayerConfiguration());
    }
}
