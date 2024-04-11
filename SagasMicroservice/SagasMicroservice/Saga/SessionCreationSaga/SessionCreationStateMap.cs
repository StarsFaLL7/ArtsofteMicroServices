using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SagasMicroservice.Saga.SessionCreationSaga;

public class SessionCreationStateMap : SagaClassMap<SessionCreationSagaState>
{
    protected override void Configure(EntityTypeBuilder<SessionCreationSagaState> entity, ModelBuilder model)
    {
        entity.Property(x => x.CurrentState).HasMaxLength(64);
        entity.Property(x => x.ResponseAddress);
    }
}