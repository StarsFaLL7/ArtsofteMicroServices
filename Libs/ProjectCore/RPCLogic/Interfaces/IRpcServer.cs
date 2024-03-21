using Microsoft.AspNetCore.Components;

namespace ProjectCore.RPCLogic.Interfaces;

public interface IRpcServer : IDisposable
{
    Task StartAsync(string queueName, Func<string, string> messageHandler);
    
    Task CloseConnectionAsync();
}