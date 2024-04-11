using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using ProjectCore.RPCLogic.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ProjectCore.RPCLogic.Services;

internal class RpcServer : IRpcServer
{
    private readonly ObjectPool<IConnection> _connectionPool;
    private readonly IServiceScopeFactory _scopeFactory;

    private IChannel _channel;
    private IConnection _connection;
    private EventingBasicConsumer _consumer;
    
    public bool IsRunning { get; private set; }
    
    
    public RpcServer(ObjectPool<IConnection> connectionPool, IServiceScopeFactory scopeFactory)
    {
        _connectionPool = connectionPool;
        _scopeFactory = scopeFactory;
    }
    
    public async Task StartAsync(string queueName, Func<string, IServiceScope, Task<string>> messageHandlerAsync)
    {
        if (IsRunning)
        {
            return;
        }
        _connection = _connectionPool.Get();
        _channel = await _connection.CreateChannelAsync();
        
        await _channel.QueueDeclareAsync(queueName, false, false, false);
        
        await _channel.BasicQosAsync(0, 1, false);
        
        _consumer = new EventingBasicConsumer(_channel);
        await _channel.BasicConsumeAsync(queueName, false, _consumer);
        IsRunning = true;
        _consumer.Received += async (model, basicDeliverEventArgs) =>
        {
            var response = string.Empty;

            var body = basicDeliverEventArgs.Body.ToArray();
            var props = basicDeliverEventArgs.BasicProperties;
            var replyProps = new BasicProperties
            {
                CorrelationId = props.CorrelationId
            };

            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var message = Encoding.UTF8.GetString(body);
                    response = await messageHandlerAsync(message, scope);
                }
                
            }
            finally
            {
                var responseBytes = Encoding.UTF8.GetBytes(response);
                await _channel.BasicPublishAsync(string.Empty, props.ReplyTo, replyProps, responseBytes);
                await _channel.BasicAckAsync(basicDeliverEventArgs.DeliveryTag, false);
            }
        };
    }

    public async Task CloseConnectionAsync()
    {
        if (!IsRunning)
        {
            return;
        }

        await _channel.CloseAsync();
        _connectionPool.Return(_connection);
        IsRunning = false;
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connectionPool.Return(_connection);
        IsRunning = false;
    }
}