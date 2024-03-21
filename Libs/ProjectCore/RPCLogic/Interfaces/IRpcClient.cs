using RabbitMQ.Client;

namespace ProjectCore.RPCLogic.Interfaces;

public interface IRpcClient : IDisposable
{
    Task<string> CallAsync(string message, string queueName, CancellationToken cancellationToken = default);
}