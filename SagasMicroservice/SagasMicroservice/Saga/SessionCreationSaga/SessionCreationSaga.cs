using CharactersCreaturesConnectionLib.DtoModels.AddItemToCharacter;
using GameSessionConnectionLib.DtoModels.GameSession.Requests;
using IdentityConnectionLib.DtoModels.CreateNotifications;
using MassTransit;
using SessionCreationService.Controllers.GameSession.Responses;

namespace SagasMicroservice.Saga.SessionCreationSaga;

public sealed class SessionCreationSaga : MassTransitStateMachine<SessionCreationSagaState>
{
    public Request<SessionCreationSagaState, SessionCreationRequest, SessionInfoResponse> CreateSessionInDatabase { get; set; }

    public Request<SessionCreationSagaState, AddItemToCharacterRequest[], AddItemToCharacterResponse[]> AddItemsToCharacters { get; set; }
    
    public Request<SessionCreationSagaState, CreateNotificationRequest[], CreateNotificationResponse[]> CreateNotifications { get; set; }
    
    public Event<SessionCreationRequest> CreateSession { get; set; }
    
    public State Failed { get; set; }


    public SessionCreationSaga()
    {
        InstanceState(x => x.CurrentState);

        Event<SessionCreationRequest>(() => CreateSession,
            x =>
                x.CorrelateById(y => y.Message.TraceId));

        Request(
            () => CreateSessionInDatabase
        );
        Request(
            () => AddItemsToCharacters
        );
        Request(
            () => CreateNotifications
        );

        Initially(
            When(CreateSession)
                .Then(x =>
                {
                    if (!x.TryGetPayload(
                            out SagaConsumeContext<SessionCreationSagaState, SessionCreationRequest> payload))
                    {
                        throw new Exception("Unable to retrieve required payload for callback data.");
                    }

                    x.Saga.InitialRequest = x.Message;
                    x.Saga.ResponseAddress = payload.ResponseAddress;
                })
                .Request(CreateSessionInDatabase, x => 
                    x.Init<SessionCreationRequest>(x.Message)
                )
                .TransitionTo(CreateSessionInDatabase.Pending)
        );

        During(CreateSessionInDatabase.Pending,
            When(CreateSessionInDatabase.Completed)
                .Then(x => x.Saga.InitialRequest.SessionId = x.Message.Id)
                .Request(AddItemsToCharacters, x => 
                    x.Init<AddItemToCharacterRequest[]>(
                        x.Saga.InitialRequest.Players
                            .Where(p => p.AdditionalItems.Length > 0)
                            .SelectMany(p => p.AdditionalItems.Select(item =>
                                new AddItemToCharacterRequest
                                {
                                    CharacterId = p.CharacterId,
                                    Title = item.Title,
                                    Count = item.Count,
                                    Description = item.Description,
                                    TraceId = x.Saga.InitialRequest.TraceId
                                })
                            )
                    )
                )
                .TransitionTo(AddItemsToCharacters.Pending),
            
            When(CreateSessionInDatabase.Faulted)
                .ThenAsync(async context => 
                    {
                        var errText = "Faulted create session in database " +
                                      string.Join("; ", context.Data.Exceptions.Select(x => x.Message));
                    
                        await RespondFromSagaAsync(context, new CreateNotificationResponse
                        {
                            Id = context.Saga.InitialRequest.SessionId,
                            Status = errText,
                            Success = false
                        });

                        throw new Exception(errText);
                    }
                )
                .TransitionTo(Failed),

            When(AddItemsToCharacters.TimeoutExpired)
                .ThenAsync(async context =>
                {
                    var errText = "Timeout expired on create session in database";
                    
                    await RespondFromSagaAsync(context, new CreateNotificationResponse
                    {
                        Id = context.Saga.InitialRequest.SessionId,
                        Status = errText,
                        Success = false
                    });
                    
                    throw new Exception(errText);
                })
                .TransitionTo(Failed)
        );
        
        
        During(AddItemsToCharacters.Pending,
            
            When(AddItemsToCharacters.Completed)
                .Request(CreateNotifications, x => x.Init<CreateNotificationRequest[]>(
                    x.Saga.InitialRequest.Players.Select(p => new CreateNotificationRequest
                    {
                        UserId = p.UserId,
                        Title = $"Вас пригласили в сессию {x.Saga.InitialRequest.Title}",
                        Content = $"Примите приглашение по ссылке: some-url.com/api/session/connect?id={x.Saga.InitialRequest.SessionId}"
                    }).ToArray()
                    )
                )
                .TransitionTo(CreateNotifications.Pending),
            
            When(AddItemsToCharacters.Faulted)
                .ThenAsync(async context =>
                {
                    var errText = "Faulted on add items to characters " + 
                                  string.Join("; ", context.Data.Exceptions.Select(x => x.Message));
                    
                    await RespondFromSagaAsync(context, new CreateNotificationResponse
                    {
                        Id = context.Saga.InitialRequest.SessionId,
                        Status = errText,
                        Success = false
                    });
                    //Тут можно сделать какие-то компенсирующие действия. 
                    //Например, вернуть деньги куда-то на счет.
                    // TODO: Delete createdSession in SessionCreationService
                    throw new Exception(errText);
                })
                .TransitionTo(Failed),
            
            
            When(AddItemsToCharacters.TimeoutExpired)
                .ThenAsync(async context =>
                {
                    var errText = "Timeout expired on add items to characters.";
                    
                    await RespondFromSagaAsync(context, new CreateNotificationResponse
                    {
                        Id = context.Saga.InitialRequest.SessionId,
                        Status = errText,
                        Success = false
                    });

                    throw new Exception(errText);
                })
                .TransitionTo(Failed)
        );

        
        During(CreateNotifications.Pending,
            When(CreateNotifications.Completed)
                .ThenAsync(async context =>  await RespondFromSagaAsync(context, new CreateNotificationResponse
                {
                    Id = context.Saga.InitialRequest.SessionId,
                    Status = "Completed",
                    Success = true
                }))
                .Finalize(),
            
            When(CreateNotifications.Faulted)
                .ThenAsync(async context =>
                {
                    //Тут можно сделать какие-то компенсирующие действия. 
                    //Например, вернуть деньги куда-то на счет.
                    //TODO: удалить предметы, удалить сессию
                    var errText = "Faulted on create notifications " +
                                  string.Join("; ", context.Data.Exceptions.Select(x => x.Message));
                    
                    await RespondFromSagaAsync(context, new CreateNotificationResponse
                    {
                        Id = context.Saga.InitialRequest.SessionId,
                        Status = errText,
                        Success = false
                    });
                    
                    throw new Exception(errText);
                })
                .TransitionTo(Failed),
            
            When(CreateNotifications.TimeoutExpired)
                .ThenAsync(async context =>
                {
                    var errText = "Timeout expired on create notifications.";
                    
                    await RespondFromSagaAsync(context, new CreateNotificationResponse
                    {
                        Id = context.Saga.InitialRequest.SessionId,
                        Status = errText,
                        Success = false
                    });

                    throw new Exception(errText);
                })
                .TransitionTo(Failed)
        );
    }

    //Метод для ответного сообщения
    //Тут нужно явно использовать ResponseAddress и RequestId 
    //сохраненные ранее чтобы ответить ровно тому, кто сделал запрос
    private static async Task RespondFromSagaAsync<T>(BehaviorContext<SessionCreationSagaState, T> context, CreateNotificationResponse response)
        where T : class
    {
        var endpoint = await context.GetSendEndpoint(context.Saga.ResponseAddress);
        await endpoint.Send(response);
    }
}