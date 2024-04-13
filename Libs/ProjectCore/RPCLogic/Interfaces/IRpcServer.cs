using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectCore.RPCLogic.Interfaces;

public interface IRpcServer : IDisposable
{
    Task StartAsync(string queueName, Func<string, IServiceScope, Task<string>> messageHandlerAsync);
    
    Task CloseConnectionAsync();
}