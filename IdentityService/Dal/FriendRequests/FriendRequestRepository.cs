using IdentityDal.FriendRequests.Interfaces;
using IdentityDal.FriendRequests.Models;
using IdentityDal.Users.Interfaces;

namespace IdentityDal.FriendRequests;

public class FriendRequestRepository : IFriendRequestRepository
{
    private readonly PostgresDbContext _dbContext;

    public FriendRequestRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<FriendRequestDal> CreateFriendRequestAsync(Guid senderId, Guid recipientId, Guid notificationId)
    {
        var model = new FriendRequestDal
        {
            SenderId = senderId,
            RecipientId = recipientId,
            CreatedDate = DateTime.Now,
            NotificationId = notificationId
        };
        _dbContext.FriendRequests.Add(model);
        return model;
    }

    public async Task DeleteFriendRequestAsync(Guid senderId, Guid recipientId)
    {
        var model = _dbContext.FriendRequests
            .FirstOrDefault(r => r.SenderId == senderId && r.RecipientId == recipientId);
        if (model != null)
        {
            _dbContext.FriendRequests.Remove(model);
            return;
        }

        throw new Exception("Запрос не найден");
    }

    public async Task<ICollection<FriendRequestDal>> GetAllFriendRequestsByUserId(Guid userId)
    {
        var result = _dbContext.FriendRequests
            .Where(r => r.SenderId == userId || r.RecipientId == userId)
            .ToList();
        return result;
    }

    public async Task<FriendRequestDal> GetRequestInfo(Guid senderId, Guid recipientId)
    {
        var result = _dbContext.FriendRequests.First(r => r.SenderId == senderId && r.RecipientId == recipientId);
        if (result == null)
        {
            throw new Exception("Запрос не найден");
        }

        return result;
    }
}