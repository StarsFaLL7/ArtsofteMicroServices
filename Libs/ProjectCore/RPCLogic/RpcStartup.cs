using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.ObjectPool;
using ProjectCore.RPCLogic.Interfaces;
using ProjectCore.RPCLogic.Services;
using RabbitMQ.Client;

namespace ProjectCore.RPCLogic;

public static class RpcStartup
{
    public static IServiceCollection TryAddRpc(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();

        serviceCollection.TryAddSingleton<IRpcClient, RpcClient>();
        
        serviceCollection.TryAddSingleton<ObjectPool<IConnection>, RpcConnectionPool>();
        
        serviceCollection.TryAddTransient<IRpcServer, RpcServer>();

        return serviceCollection;
    }
}