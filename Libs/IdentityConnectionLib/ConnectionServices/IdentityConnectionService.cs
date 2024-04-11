using IdentityConnectionLib.DtoModels.CreateNotifications;
using IdentityConnectionLib.DtoModels.UserNameList;
using IdentityConnectionLib.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using Polly;
using ProjectCore.Enums;
using ProjectCore.HttpLogic;
using ProjectCore.HttpLogic.Enums;
using ProjectCore.HttpLogic.Services.Interfaces;
using ProjectCore.RPCLogic.Interfaces;
using ProjectCore.RPCLogic.Services;
using RabbitMQ.Client;
using HttpRequestData = ProjectCore.HttpLogic.Models.HttpRequestData;

namespace IdentityConnectionLib.ConnectionServices;

public class IdentityConnectionService : IIdentityConnectionService
{
    private readonly IHttpRequestService? _httpRequestService;
    private readonly string _httpHost = null!;

    private readonly IRpcClient? _rpcClient;
    private readonly string _queueUserNames = null!;
    private readonly string _queueCreateNotification = null!;
    
    private ConnectionType _connectionType;
    
    public IdentityConnectionService(IConfiguration configuration, IServiceProvider serviceProvider)
    {
        var infoSection = configuration.GetSection("IdentityConnectionLib");
        if (infoSection.GetSection("Method").Value == "rpc")
        {
            var rpcInfoSection = infoSection.GetSection("Rpc");
            _queueUserNames = rpcInfoSection.GetValue<string>("QueueGetUserNames");
            _queueCreateNotification = rpcInfoSection.GetValue<string>("QueueCreateNotification");
            
            _rpcClient = serviceProvider.GetRequiredService<IRpcClient>();
            _connectionType = ConnectionType.Rpc;
        }
        else
        {
            var httpInfoSection = infoSection.GetSection("Http");
            var hostnameHttp = httpInfoSection.GetValue<string>("Host");
            var portHttp = httpInfoSection.GetValue<int>("Port");
            _httpHost = $"http://{hostnameHttp}:{portHttp}/";
            
            _httpRequestService = serviceProvider.GetRequiredService<IHttpRequestService>();
            _connectionType = ConnectionType.Http;
        }
    }

    public async Task<UsernameIdentityApiResponse> GetUserNameListAsync(UsernameIdentityApiRequest apiRequest)
    {
        if (_connectionType == ConnectionType.Rpc)
        {
            return await GetUserNameListAsyncByRpc(apiRequest);
        }
        if (_connectionType == ConnectionType.Http)
        {
            return await GetUserNameListAsyncByHttp(apiRequest);
        }

        throw new Exception($"Тип соединения не поддерживается: {_connectionType}");
    }

    public async Task<CreateNotificationResponse[]> CreateNotificationForUserAsync(CreateNotificationRequest[] apiRequest)
    {
        if (_connectionType == ConnectionType.Rpc)
        {
            return await CreateNotificationByRpc(apiRequest);
        }
        if (_connectionType == ConnectionType.Http)
        {
            return await CreateNotificationByHttp(apiRequest);
        }

        throw new Exception($"Тип соединения не поддерживается: {_connectionType}");
    }
    
    private async Task<CreateNotificationResponse[]> CreateNotificationByRpc(CreateNotificationRequest[] apiRequest)
    {
        var message = JsonConvert.SerializeObject(apiRequest);
        var policy = Policy.Timeout(5);
        var response = await policy.Execute(async () => await _rpcClient.CallAsync(message, _queueCreateNotification));
        var result = JsonConvert.DeserializeObject<CreateNotificationResponse[]>(response);
        
        return result;
    }
    
    private async Task<CreateNotificationResponse[]> CreateNotificationByHttp(CreateNotificationRequest[] apiRequest)
    {
        var requestData = new HttpRequestData
        {
            Method = HttpMethod.Post,
            Uri = new Uri(_httpHost + "api/notifications/create"),
            ContentType = ContentType.ApplicationJson,
            Body = apiRequest
        };
        
        var retryPolicy = PollySettings.GetRetryPolicyForHttp(5, i => TimeSpan.FromSeconds(2 * i));
        var res = await _httpRequestService.SendRequestAsync<CreateNotificationResponse[]>(requestData, retryPolicy);
        
        return res.Body;
    }

    private async Task<UsernameIdentityApiResponse> GetUserNameListAsyncByRpc(UsernameIdentityApiRequest apiRequest)
    {
        var message = JsonConvert.SerializeObject(apiRequest);
        var policy = Policy.Timeout(5);
        var response = await policy.Execute(async () => await _rpcClient.CallAsync(message, _queueUserNames));
        var result = JsonConvert.DeserializeObject<UsernameIdentityApiResponse>(response);
        
        return result;
    }
    
    private async Task<UsernameIdentityApiResponse> GetUserNameListAsyncByHttp(UsernameIdentityApiRequest apiRequest)
    {
        var requestData = new HttpRequestData
        {
            Method = HttpMethod.Post,
            Uri = new Uri(_httpHost + "api/user/usernames"),
            ContentType = ContentType.ApplicationJson,
            Body = apiRequest
        };
        
        var retryPolicy = PollySettings.GetRetryPolicyForHttp(5, i => TimeSpan.FromSeconds(2 * i));
        var res = await _httpRequestService.SendRequestAsync<UsernameIdentityApiResponse>(requestData, retryPolicy);
        
        return res.Body;
    }
}