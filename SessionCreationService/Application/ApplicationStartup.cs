using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Application;

public static class ApplicationStartup
{
    public static IServiceCollection TryAddApplicationLayer(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddScoped<IGameSessionService, GameSessionService>();
        return serviceCollection;
    }
}