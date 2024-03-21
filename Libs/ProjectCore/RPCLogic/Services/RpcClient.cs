using System.Collections.Concurrent;
using System.Text;
using Microsoft.Extensions.ObjectPool;
using ProjectCore.RPCLogic.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ProjectCore.RPCLogic.Services;

internal class RpcClient : IRpcClient
{
    private readonly ObjectPool<IConnection> _connectionPool;
    private IConnection _connection;
    private IChannel _channel;
    private string _replyQueueName;
    private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _callbackMapper = new ();
    
    public RpcClient(ObjectPool<IConnection> connectionPool)
    {
        _connectionPool = connectionPool;
        Task.Run(async () => await Init());
    }

    private async Task Init()
    {
        var connection = _connectionPool.Get();
        var channel = await connection.CreateChannelAsync();
        var replyQueueName = (await channel.QueueDeclareAsync()).QueueName;
        _connection = connection;
        _channel = channel;
        _replyQueueName = replyQueueName;
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            if (!_callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out var tcs))
                return;
            var body = ea.Body.ToArray();
            var response = Encoding.UTF8.GetString(body);
            tcs.TrySetResult(response);
        };

        await _channel.BasicConsumeAsync(consumer: consumer,
            queue: _replyQueueName,
            autoAck: true);
    }

    public async Task<string> CallAsync(string message, string queueName, CancellationToken cancellationToken = default)
    {
        var props = new BasicProperties();
        var correlationId = Guid.NewGuid().ToString();
        props.CorrelationId = correlationId;
        props.ReplyTo = _replyQueueName;
        var messageBytes = Encoding.UTF8.GetBytes(message);
        var tcs = new TaskCompletionSource<string>();
        _callbackMapper.TryAdd(correlationId, tcs);

        await _channel.BasicPublishAsync(exchange: string.Empty,
                             routingKey: queueName,
                             basicProperties: props,
                             body: messageBytes);

        cancellationToken.Register(() => _callbackMapper.TryRemove(correlationId, out _));
        return await tcs.Task;
    }

    public async void Dispose()
    {
        await _channel.CloseAsync();
        _connectionPool.Return(_connection);
    }
}