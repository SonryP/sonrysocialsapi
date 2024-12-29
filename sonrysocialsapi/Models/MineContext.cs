using Microsoft.EntityFrameworkCore;

namespace sonrysocialsapi.Models;

public class MineContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Server> Servers { get; set; }
    public DbSet<Activation> Activations { get; set; }
    public DbSet<Share> Shares { get; set; }

    public MineContext(DbContextOptions<MineContext> options)  : base(options)
    {
    }
}