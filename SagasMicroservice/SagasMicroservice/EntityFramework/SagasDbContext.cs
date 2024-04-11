using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using SagasMicroservice.Saga.SessionCreationSaga;

namespace SagasMicroservice.EntityFramework;

public sealed class SagasDbContext : SagaDbContext
{
    public SagasDbContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get
        {
            yield return new SessionCreationStateMap();
        }
    }
}