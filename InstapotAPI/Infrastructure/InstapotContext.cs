using InstapotAPI.Entity;
using Microsoft.EntityFrameworkCore;
namespace InstapotAPI.Infrastructure;

public class InstapotContext : DbContext
{
    public InstapotContext(DbContextOptions<InstapotContext> options) : base(options)
    {
        
    }

    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Comment> Comments { get; set; }
}
