using Microsoft.EntityFrameworkCore;

namespace Server.Module.Player.Infrastructure.EfCore;

public class Context : DbContext
{
    public Context(DbContextOptions options) : base(options)
    {
    }
}
