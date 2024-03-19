using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using ProjectCore.HttpLogic.Models;
using ProjectCore.HttpLogic.Services.Interfaces;
using ProjectCore.TraceIdLogic.Interfaces;
using ContentType = ProjectCore.HttpLogic.Enums.ContentType;

namespace ProjectCore.HttpLogic.Services;

/// <inheritdoc />
internal class HttpRequestService : IHttpRequestService
{
    private readonly IHttpConnectionService _httpConnectionService;
    private readonly IEnumerable<ITraceWriter> _traceWriterList;
    
    public HttpRequestService(
        IHttpConnectionService httpConnectionService,
        IEnumerable<ITraceWriter> traceWriterList)
    {
        _httpConnectionService = httpConnectionService;
        _traceWriterList = traceWriterList;
    }

    /// <inheritdoc />
    public async Task<HttpResponse<TResponse>> SendRequestAsync<TResponse>(HttpRequestData requestData, IAsyncPolicy<HttpResponseMessage> policy,
        HttpConnectionData connectionData)
    {
        var client = _httpConnectionService.CreateHttpClient(connectionData);
        var httpRequestMessage = new HttpRequestMessage
        {
            Content = PrepairContent(requestData.Body, requestData.ContentType),
            Method = requestData.Method,
            RequestUri = GetUriWithQuery(requestData.Uri, requestData.QueryParameterList)
        };
        
        foreach (var headerKvPair in requestData.HeaderDictionary)
        {
            httpRequestMessage.Headers.Add(headerKvPair.Key, headerKvPair.Value);
        }
        
        foreach (var traceWriter in _traceWriterList)
        {
            httpRequestMessage.Headers.Add(traceWriter.Name, traceWriter.GetValue());
        }

        var responseMessage = await policy.ExecuteAsync(async (token) =>
        {
            token.ThrowIfCancellationRequested();
            var msg = CloneHttpRequestMessage(httpRequestMessage);
            var res = await _httpConnectionService.SendRequestAsync(msg, client, default);
            return res;
        }, connectionData.CancellationToken);
        
        responseMessage.EnsureSuccessStatusCode();
        
        var contentString = await responseMessage.Content.ReadAsStringAsync();
        var resultObject = JsonConvert.DeserializeObject<TResponse>(contentString);
        return new HttpResponse<TResponse>()
        {
            Body = resultObject,
            Headers = responseMessage.Headers,
            StatusCode = responseMessage.StatusCode,
            ContentHeaders = responseMessage.Content.Headers
        };
    }

    private static Uri GetUriWithQuery(Uri uri, ICollection<KeyValuePair<string, string>> queryParameterList)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(uri);
        var isFirstQuery = true;
        foreach (var queryKvPair in queryParameterList)
        {
            if (isFirstQuery)
            {
                stringBuilder.Append('?');
                isFirstQuery = false;
            }
            else
            {
                stringBuilder.Append('&');
            }
            stringBuilder.Append(queryKvPair.Key);
            stringBuilder.Append('=');
            stringBuilder.Append(queryKvPair.Value);
        }

        return new Uri(stringBuilder.ToString());
    }
    
    private HttpRequestMessage CloneHttpRequestMessage(HttpRequestMessage message)
    {
        var msg = new HttpRequestMessage
        {
            Content = message.Content,
            Method = message.Method,
            RequestUri = message.RequestUri,
            Version = message.Version,
            VersionPolicy = message.VersionPolicy,
        };
        foreach (var header in message.Headers)
        {
            msg.Headers.Add(header.Key, header.Value);
        }

        return msg;
    }

    private static HttpContent PrepairContent(object body, ContentType contentType)
    {
        switch (contentType)
        {
            case ContentType.ApplicationJson:
            {
                if (body is string stringBody)
                {
                    body = JToken.Parse(stringBody);
                }

                var serializeSettings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                };
                var serializedBody = JsonConvert.SerializeObject(body, serializeSettings);
                var content = new StringContent(serializedBody, Encoding.UTF8, MediaTypeNames.Application.Json);
                return content;
            }

            case ContentType.XWwwFormUrlEncoded:
            {
                if (body is not IEnumerable<KeyValuePair<string, string>> list)
                {
                    throw new Exception(
                        $"Body for content type {contentType} must be {typeof(IEnumerable<KeyValuePair<string, string>>).Name}");
                }

                return new FormUrlEncodedContent(list);
            }
            case ContentType.ApplicationXml:
            {
                if (body is not string s)
                {
                    throw new Exception($"Body for content type {contentType} must be XML string");
                }

                return new StringContent(s, Encoding.UTF8, MediaTypeNames.Application.Xml);
            }
            case ContentType.Binary:
            {
                if (body.GetType() != typeof(byte[]))
                {
                    throw new Exception($"Body for content type {contentType} must be {typeof(byte[]).Name}");
                }

                return new ByteArrayContent((byte[])body);
            }
            case ContentType.TextXml:
            {
                if (body is not string s)
                {
                    throw new Exception($"Body for content type {contentType} must be XML string");
                }

                return new StringContent(s, Encoding.UTF8, MediaTypeNames.Text.Xml);
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(contentType), contentType, null);
        }
    }
}