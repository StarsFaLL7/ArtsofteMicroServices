using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Infrastructure;

public static class InfrastructureStartup
{
    public static IServiceCollection TryAddInfrastructure(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddScoped<IStoreGameSession, GameSessionRepository>();
        serviceCollection.TryAddScoped<IStoreCreatureInSession, CreatureInSessionRepository>();
        serviceCollection.TryAddScoped<IStoreCharacterInSession, CharacterIsSessionRepository>();
        serviceCollection.AddDbContext<PostgresDbContext>();
        return serviceCollection;
    }
}