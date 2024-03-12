using Domain.Interfaces;
using Infrastructure.Connections;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
namespace Infrastructure;

public static class InfrastructureStartup
{
    public static IServiceCollection TryAddInfrastructure(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddScoped<IStoreAbility, AbilityRepository>();
        serviceCollection.TryAddSingleton<IStoreCharacteristicsSet, CharacteristicsSetRepository>();
        serviceCollection.TryAddScoped<IStoreCharacter, CharacterRepository>();
        serviceCollection.TryAddSingleton<IStoreCreature, CreatureRepository>();
        serviceCollection.TryAddScoped<IStoreInventoryItem, InventoryItemRepository>();
        serviceCollection.TryAddScoped<IStoreSkillSet, SkillSetRepository>();
        
        serviceCollection.TryAddScoped<ICreatureUserConnection, CreatureUserConnection>();
        return serviceCollection;
    }
}