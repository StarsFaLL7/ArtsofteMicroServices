using CharactersCreaturesConnectionLib.DtoModels.AddItemToCharacter;
using CharactersCreaturesConnectionLib.DtoModels.GetCharacterInfo;
using CharactersCreaturesConnectionLib.DtoModels.GetCreatureInfo;
using CharactersCreaturesConnectionLib.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Polly;
using ProjectCore.Enums;
using ProjectCore.HttpLogic;
using ProjectCore.HttpLogic.Enums;
using ProjectCore.HttpLogic.Models;
using ProjectCore.HttpLogic.Services.Interfaces;
using ProjectCore.RPCLogic.Interfaces;

namespace CharactersCreaturesConnectionLib.ConnectionServices;

internal class CharacterCreaturesConnectionService : ICharactersCreaturesConnectionService
{
    private readonly string _httpHost;
    private readonly string _queueAddItemToCharacter;
    private readonly string _queueGetCharacterInfo;
    private readonly string _queueGetCreatureInfo;
    
    private readonly ConnectionType _connectionType;
    private readonly IRpcClient _rpcClient;
    private readonly IHttpRequestService _httpRequestService;
    
    public CharacterCreaturesConnectionService(IConfiguration configuration, IServiceProvider serviceProvider)
    {
        var infoSection = configuration.GetSection("CharactersCreaturesConnectionLib");
        
        var httpInfoSection = infoSection.GetSection("Http");
        var hostnameHttp = httpInfoSection.GetValue<string>("Host");
        var portHttp = httpInfoSection.GetValue<int>("Port");
        _httpHost = $"http://{hostnameHttp}:{portHttp}/";

        var rpcInfoSection = infoSection.GetSection("Rpc");
        _queueAddItemToCharacter = rpcInfoSection.GetValue<string>("QueueAddItemToCharacter");
        _queueGetCharacterInfo = rpcInfoSection.GetValue<string>("QueueGetCharacterInfo");
        _queueGetCreatureInfo = rpcInfoSection.GetValue<string>("QueueGetCreatureInfo");
        
        if (infoSection.GetSection("Method").Value == "rpc")
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
    
    public async Task<AddItemToCharacterResponse[]> AddItemToCharacter(AddItemToCharacterRequest[] request)
    {
        if (_connectionType == ConnectionType.Rpc)
        {
            return await SendRpcRequest<AddItemToCharacterRequest[], AddItemToCharacterResponse[]>(request, _queueAddItemToCharacter);
        }
        if (_connectionType == ConnectionType.Http)
        {
            return await SendPostHttpRequest<AddItemToCharacterRequest[], AddItemToCharacterResponse[]>(request, 
                new Uri(_httpHost + "api/internal/characters/items"));
        }

        throw new Exception($"Тип соединения не поддерживается: {_connectionType}");
    }

    public async Task<GetCharacterInfoResponse[]> GetCharacterInfo(GetCharacterInfoRequest request)
    {
        if (_connectionType == ConnectionType.Rpc)
        {
            return await SendRpcRequest<GetCharacterInfoRequest, GetCharacterInfoResponse[]>(request, _queueGetCharacterInfo);
        }
        if (_connectionType == ConnectionType.Http)
        {
            return await SendPostHttpRequest<GetCharacterInfoRequest, GetCharacterInfoResponse[]>(request, 
                new Uri(_httpHost + "api/internal/characters"));
        }

        throw new Exception($"Тип соединения не поддерживается: {_connectionType}");
    }

    public async Task<GetCreatureInfoResponse[]> GetCreatureInfo(GetCreatureInfoRequest request)
    {
        if (_connectionType == ConnectionType.Rpc)
        {
            return await SendRpcRequest<GetCreatureInfoRequest, GetCreatureInfoResponse[]>(request, _queueGetCreatureInfo);
        }
        if (_connectionType == ConnectionType.Http)
        {
            return await SendPostHttpRequest<GetCreatureInfoRequest, GetCreatureInfoResponse[]>(request, 
                new Uri(_httpHost + "api/internal/creatures"));
        }

        throw new Exception($"Тип соединения не поддерживается: {_connectionType}");
    }

    private async Task<TResponse> SendRpcRequest<TRequest, TResponse>(TRequest request, string queueName)
    {
        var message = JsonConvert.SerializeObject(request);
        var policy = Policy.Timeout(5);
        var response = await policy.Execute(async () => await _rpcClient.CallAsync(message, queueName));
        var result = JsonConvert.DeserializeObject<TResponse>(response);
        return result;
    }

    private async Task<TResponse> SendPostHttpRequest<TRequest, TResponse>(TRequest request, Uri uri)
    {
        var requestData = new HttpRequestData
        {
            Method = HttpMethod.Post,
            Uri = uri,
            ContentType = ContentType.ApplicationJson,
            Body = request
        };
        var retryPolicy = PollySettings.GetRetryPolicyForHttp(5, i => TimeSpan.FromSeconds(2 * i));
        var res = await _httpRequestService.SendRequestAsync<TResponse>(requestData, retryPolicy);
        return res.Body;
    }
}