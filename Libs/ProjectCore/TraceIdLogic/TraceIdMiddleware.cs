using Microsoft.AspNetCore.Http;
using ProjectCore.TraceIdLogic.Interfaces;

namespace ProjectCore.TraceIdLogic;

public class TraceIdMiddleware
{
    private readonly RequestDelegate _next;
    //private readonly IEnumerable<ITraceWriter> _traceIdWriterList;

    public TraceIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }
 
    public async Task InvokeAsync(HttpContext context, IEnumerable<ITraceReader> traceIdReaderList)
    {
        foreach (var traceWriter in traceIdReaderList)
        {
            if (context.Request.Headers.ContainsKey(traceWriter.Name))
            {
                traceWriter.WriteValue(context.Request.Headers[traceWriter.Name]);
            }
            else
            {
                var traceId = Guid.NewGuid().ToString();
                traceWriter.WriteValue(traceId);
                context.Request.Headers.Append(traceWriter.Name, traceId);
            }
        }
        
        await _next.Invoke(context);
    }
}