using ProjectCore.HttpLogic.Models.Base;

namespace ProjectCore.HttpLogic.Models;

public record HttpResponse<TResponse> : BaseHttpResponse
{
    /// <summary>
    /// Тело ответа
    /// </summary>
    public TResponse Body { get; set; }
}