using IdentityDal.FriendRequests.Models;
using IdentityDal.Notifications.Models;
using IdentityDal.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IdentityDal;

public class PostgresDbContext : DbContext
{
    private readonly string _connectionString;
    
    public DbSet<UserDal> Users { get; set; }
    //public DbSet<UserUserConnection> UserUserConnections { get; set; }
    public DbSet<NotificationDal> Notifications { get; set; }
    public DbSet<FriendRequestDal> FriendRequests { get; set; }
    
    public PostgresDbContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DockerConnection");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserDal>().HasMany<UserDal>(u => u.Friends).WithMany();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }
}