using IdentityConnectionLib.DtoModels.UserNameList;
using IdentityConnectionLib.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectCore.HttpLogic;
using ProjectCore.HttpLogic.Enums;
using ProjectCore.HttpLogic.Services.Interfaces;
using HttpRequestData = ProjectCore.HttpLogic.Models.HttpRequestData;

namespace IdentityConnectionLib.ConnectionServices;

public class IdentityConnectionService : IIdentityConnectionService
{
    private readonly IHttpRequestService _httpRequestService;
    private readonly string _httpHost = "http://localhost:5136/";
    
    public IdentityConnectionService(IConfiguration configuration, IServiceProvider serviceProvider)
    {
        _httpRequestService = serviceProvider.GetRequiredService<IHttpRequestService>();
    }
    
    public async Task<UsernameIdentityApiResponse> GetUserNameListAsync(UsernameIdentityApiRequest apiRequest)
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