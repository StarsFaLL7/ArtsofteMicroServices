using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data;

public class PostgresDbContext : DbContext
{
    private readonly string _connectionString;

    public DbSet<CharacterInSession> CharacterInSessions { get; set; }
    public DbSet<CreatureInSession> CreatureInSessions { get; set; }
    public DbSet<GameSession> GameSessions { get; set; }
    
    public PostgresDbContext(IConfiguration configuration)
    {
        var sectionToUse = configuration.GetSection("ConnectionStringToUse").Value;
        _connectionString = configuration.GetConnectionString(sectionToUse);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }
}