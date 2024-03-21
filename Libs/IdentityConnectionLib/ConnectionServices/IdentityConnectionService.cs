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
    private const string HttpHost = "http://localhost:5136/";

    private readonly IRpcClient? _rpcClient;
    private const string QueueUserNames = "rpc_userNames";

    private ConnectionType _connectionType;
    
    public IdentityConnectionService(IConfiguration configuration, IServiceProvider serviceProvider)
    {
        if (configuration.GetSection("IdentityConnectionMethod").Value == "rpc")
        {
            _rpcClient = serviceProvider.GetRequiredService<IRpcClient>();
            _connectionType = ConnectionType.Rpc;
        }
        else
        {
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
    
    private async Task<UsernameIdentityApiResponse> GetUserNameListAsyncByRpc(UsernameIdentityApiRequest apiRequest)
    {
        var message = JsonConvert.SerializeObject(apiRequest);
        var policy = Policy.Timeout(5);
        var response = await policy.Execute(async () => await _rpcClient.CallAsync(message, QueueUserNames));
        var result = JsonConvert.DeserializeObject<UsernameIdentityApiResponse>(response);
        
        return result;
    }
    
    private async Task<UsernameIdentityApiResponse> GetUserNameListAsyncByHttp(UsernameIdentityApiRequest apiRequest)
    {
        var requestData = new HttpRequestData
        {
            Method = HttpMethod.Post,
            Uri = new Uri(HttpHost + "api/user/usernames"),
            ContentType = ContentType.ApplicationJson,
            Body = apiRequest
        };
        
        var retryPolicy = PollySettings.GetRetryPolicyForHttp(5, i => TimeSpan.FromSeconds(2 * i));
        var res = await _httpRequestService.SendRequestAsync<UsernameIdentityApiResponse>(requestData, retryPolicy);
        
        return res.Body;
    }
}