namespace IdentityConnectionLib.DtoModels.CreateNotifications;

public class CreateNotificationResponse
{
    public required Guid Id { get; set; }
    
    public required string Status { get; set; }
    
    public required bool Success { get; set; }
}