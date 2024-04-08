using IdentityConnectionLib.DtoModels.UserNameList;
using IdentityLogic.Users.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using ProjectCore.RPCLogic.Interfaces;
using ProjectCore.RPCLogic.Services;

namespace IdentityApi.RpcApi;

public static class RpcServerStartup
{
    public static IApplicationBuilder UseRpcServer(this IApplicationBuilder builder)
    {
        var server = builder.ApplicationServices.GetRequiredService<IRpcServer>();
        server.StartAsync("rpc_userNames", (msg, scope) =>
        {
            var dto = JsonConvert.DeserializeObject<UsernameIdentityApiRequest>(msg);
            var userManager = scope.ServiceProvider.GetRequiredService<IUserLogicManager>();
            var namesArray = Task.Run(async () => await userManager.GetUsernamesByIdsAsync(dto.UserIds)).Result;
            var result = new UsernameIdentityApiResponse
            {
                Usernames = namesArray
            };
            return JsonConvert.SerializeObject(result);
        });
        return builder;
    }
}