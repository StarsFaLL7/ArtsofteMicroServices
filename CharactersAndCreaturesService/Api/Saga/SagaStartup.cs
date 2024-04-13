using MassTransit;

namespace Api.Saga;

public static class SagaStartup
{
    public static IServiceCollection AddSaga(this IServiceCollection services)
    {
        services.AddMassTransit(cfg =>
        {
            cfg.SetKebabCaseEndpointNameFormatter();

            cfg.AddDelayedMessageScheduler();

            cfg.AddConsumer<AddItemsToCharacterConsumer>();

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