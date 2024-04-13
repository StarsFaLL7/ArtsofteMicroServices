using GameSessionConnectionLib.DtoModels.GameSession.Requests;
using GameSessionConnectionLib.DtoModels.GameSession.Responses;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SessionCreationService.Controllers.GameSession.Responses;

namespace SagasMicroservice.Controllers;

[Route("api/session-create")]
public class SessionsController : ControllerBase
{
    private readonly IBus _bus;

    public SessionsController(IBus bus)
    {
        _bus = bus;
    }
    
    [HttpPost]
    [ProducesResponseType<SessionCreationResponse>(200)]
    public async Task<IActionResult> CreateSession([FromBody] SessionCreationRequest request)
    {
        var response = await _bus.Request<SessionCreationRequest, SessionCreationResponse>(request);
        return Ok(response.Message);
    }
}