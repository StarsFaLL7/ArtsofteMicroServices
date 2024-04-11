using IdentityConnectionLib.DtoModels.CreateNotifications;
using IdentityConnectionLib.DtoModels.UserNameList;
using IdentityLogic.Notifications.Interfaces;
using IdentityLogic.Notifications.Models;
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
        var serverUserNames = builder.ApplicationServices.GetRequiredService<IRpcServer>();
        serverUserNames.StartAsync("rpc_userNames", async (msg, scope) =>
        {
            var dto = JsonConvert.DeserializeObject<UsernameIdentityApiRequest>(msg);
            var userManager = scope.ServiceProvider.GetRequiredService<IUserLogicManager>();
            var namesArray = await userManager.GetUsernamesByIdsAsync(dto.UserIds);
            var result = new UsernameIdentityApiResponse
            {
                Usernames = namesArray
            };
            return JsonConvert.SerializeObject(result);
        });
        var serverNotifications = builder.ApplicationServices.GetRequiredService<IRpcServer>();
        serverNotifications.StartAsync("rpc_createNotification", async (msg, scope) =>
        {
            var dto = JsonConvert.DeserializeObject<CreateNotificationRequest[]>(msg);
            var notificationManager = scope.ServiceProvider.GetRequiredService<INotificationManager>();
            var userManager = scope.ServiceProvider.GetRequiredService<IUserLogicManager>();

            var ids = new List<Guid>();
            foreach (var notif in dto)
            {
                var notificationId = await notificationManager.CreateNewNotificationAsync(new NotificationLogic
                {
                    Id = Guid.NewGuid(),
                    UserId = notif.UserId,
                    Title = notif.Title,
                    Content = notif.Content,
                    WasRead = false,
                    CreatedTime = DateTime.Now
                });
                ids.Add(notificationId);
            }

            var userNames = await userManager.GetUsernamesByIdsAsync(dto.Select(n => n.UserId).ToArray());
            var result = ids.Select((id, i) => new CreateNotificationResponse
            {
                Id = id,
                Username = userNames[i]
            }).ToArray();
            return JsonConvert.SerializeObject(result);
        });
        return builder;
    }
}