using System.Reflection;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using SagasMicroservice.EntityFramework;
using SagasMicroservice.Saga.SessionCreationSaga;

namespace SagasMicroservice.Saga;

public static class SagaStartup
{
    public static IServiceCollection AddSaga(this IServiceCollection services)
    {
        services.AddMassTransit(cfg =>
        {
            cfg.SetKebabCaseEndpointNameFormatter();
            cfg.AddDelayedMessageScheduler();
            //Тут добавляем сагу с указанием что будем сохранять ее в БД 
            //с помощью EF и будем использовать пессимистичный режим конкуренции за ресурсы
            cfg.AddSagaStateMachine<SessionCreationSaga.SessionCreationSaga, SessionCreationSagaState>()
                .EntityFrameworkRepository(r =>
                {
                    r.ConcurrencyMode = ConcurrencyMode.Pessimistic;
                    r.AddDbContext<DbContext, SagasDbContext>((provider,builder) =>
                    {
                        builder.UseNpgsql("Host=localhost;Port=5433;Database=sagas;Username=postgres;Password=secretpassword123;", m =>
                        {
                            m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                            m.MigrationsHistoryTable($"__{nameof(SagasDbContext)}");
                        });
                    });
                    r.UsePostgres();
                    r.LockStatementProvider = new PostgresLockStatementProvider();
                });
            cfg.UsingRabbitMq((brc, rbfc) =>
            {
                rbfc.UseInMemoryOutbox();
                rbfc.UseMessageRetry(r =>
                {
                    r.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
                });
                rbfc.UseDelayedMessageScheduler();
                rbfc.Host("localhost", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                rbfc.ConfigureEndpoints(brc);
            });
        });
        
        return services;
    }
}