using System.Net;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace ProjectCore.HttpLogic;

public static class PollySettings
{
    /// <summary>
    /// Получить новый экземпляр IAsyncPolicy, TResult = HttpResponseMessage с заданными параметрами. 
    /// </summary>
    /// <param name="retryCount">Кол-во попыток выполнения запроса</param>
    /// <param name="sleepDurationProvider">Функция подсчёта время задержки между попытками, входной параметр int - номер попытки</param>
    /// <returns></returns>
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicyForHttp(int retryCount, Func<int, TimeSpan> sleepDurationProvider)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(retryCount, sleepDurationProvider);
    }
}

