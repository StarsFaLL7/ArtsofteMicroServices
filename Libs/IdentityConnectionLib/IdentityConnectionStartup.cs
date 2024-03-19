using IdentityConnectionLib.ConnectionServices;
using IdentityConnectionLib.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using ProjectCore.HttpLogic.Services.Interfaces;

namespace IdentityConnectionLib;

public static class IdentityConnectionServiceStartup
{
    /// <summary>
    /// Добавление сервиса для осуществления запросов по HTTP
    /// </summary>
    public static IServiceCollection AddIdentityConnectionService(this IServiceCollection services)
    {
        services.AddScoped<IIdentityConnectionService, IdentityConnectionService>();
        
        return services;
    }
}