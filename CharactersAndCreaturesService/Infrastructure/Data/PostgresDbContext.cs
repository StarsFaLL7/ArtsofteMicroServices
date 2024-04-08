using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data;

public class PostgresDbContext : DbContext
{
    private readonly string _connectionString;
    
    public DbSet<Ability> Abilities { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<CharacteristicsSet> CharacteristicsSets { get; set; }
    public DbSet<Creature> Creatures { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<SkillSet> SkillSets { get; set; }
    
    public PostgresDbContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DockerConnection");
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }
}