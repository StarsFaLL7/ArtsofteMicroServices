using CharactersCreaturesConnectionLib.ConnectionServices;
using CharactersCreaturesConnectionLib.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CharactersCreaturesConnectionLib;

public static class CharacterCreaturesConnectionStartup
{
    public static IServiceCollection AddCharacterCreaturesConnectionService(this IServiceCollection services)
    {
        services.AddScoped<ICharactersCreaturesConnectionService, CharacterCreaturesConnectionService>();
        return services;
    }
}