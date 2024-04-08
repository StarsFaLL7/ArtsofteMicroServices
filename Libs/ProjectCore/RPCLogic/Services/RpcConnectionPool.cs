using System.Collections.Concurrent;
using System.Security.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

namespace ProjectCore.RPCLogic.Services;

public class RpcConnectionPool : ObjectPool<IConnection>
{
    private readonly ConcurrentBag<IConnection> _connections = new ();
    private readonly ConnectionFactory _connectionFactory;
    
    public RpcConnectionPool(IConfiguration configuration)
    {
        var rpcSection = configuration.GetSection("ConnectionLib").GetSection("Rpc");
        var host = rpcSection.GetValue<string>("Host");
        var port = rpcSection.GetValue<int>("Port");

        _connectionFactory = new ConnectionFactory
        {
            HostName = host,
            Port = port,
        };
    }
    
    public override IConnection Get()
    {
        if (_connections.TryTake(out var connection))
        {
            return connection;
        }

        var newConnection = _connectionFactory.CreateConnectionAsync().GetAwaiter().GetResult();
        return newConnection;
    }
    
    public override void Return(IConnection obj)
    {
        _connections.Add(obj);
    }
}