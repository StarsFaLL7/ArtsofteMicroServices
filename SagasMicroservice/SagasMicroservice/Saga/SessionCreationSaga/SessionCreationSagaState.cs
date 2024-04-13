using GameSessionConnectionLib.DtoModels.GameSession.Requests;
using MassTransit;

namespace SagasMicroservice.Saga.SessionCreationSaga;

public sealed class SessionCreationSagaState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    
    public string? CurrentState { get; set; }
    //Тут мы сохраняем адрес откуда пришел запрос который запустил нашу сагу
    //чтобы ответить на него
    public Uri? ResponseAddress { get; set; }
    
    public SessionCreationRequest InitialRequest { get; set; }
}