using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
namespace Infrastructure;

public static class InfrastructureStartup
{
    public static IServiceCollection TryAddInfrastructure(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddScoped<IStoreAbility, AbilityRepository>();
        serviceCollection.TryAddScoped<IStoreCharacteristicsSet, CharacteristicsSetRepository>();
        serviceCollection.TryAddScoped<IStoreCharacter, CharacterRepository>();
        serviceCollection.TryAddScoped<IStoreCreature, CreatureRepository>();
        serviceCollection.TryAddScoped<IStoreInventoryItem, InventoryItemRepository>();
        serviceCollection.TryAddScoped<IStoreSkillSet, SkillSetRepository>();
        return serviceCollection;
    }
}