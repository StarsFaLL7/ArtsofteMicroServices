using ProjectCore.Api.Responses;

namespace Api.Controllers.OtherResponses.Items;

public class ItemsArrayStatusResponse : StatusResponse
{
    public required InventoryItemInfoResponse[] Items { get; init; }
}