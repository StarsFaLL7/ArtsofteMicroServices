using System.Collections.Concurrent;
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
        _connectionFactory = new ConnectionFactory();
    }
    
    public override IConnection Get()
    {
        if (_connections.TryTake(out var connection))
        {
            return connection;
        }

        var newConnection = _connectionFactory.CreateConnectionAsync().Result;
        return newConnection;
    }

    public override void Return(IConnection obj)
    {
        _connections.Add(obj);
    }
}