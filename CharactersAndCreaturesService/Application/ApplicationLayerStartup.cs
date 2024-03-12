using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Application;

public static class ApplicationLayerStartup
{
    public static IServiceCollection TryAddApplicationLayer(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddScoped<IAbilityService, AbilityService>();
        serviceCollection.TryAddScoped<ICreatureService, CreatureService>();
        serviceCollection.TryAddScoped<ICharacterService, CharacterService>();
        
        return serviceCollection;
    }
}