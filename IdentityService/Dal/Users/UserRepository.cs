using System.Collections.Concurrent;
using IdentityDal.Users.Interfaces;
using IdentityDal.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityDal.Users;

/// <inheritdoc />
internal class UserRepository : IUserRepository
{
    private readonly PostgresDbContext _dbContext;

    public UserRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> CreateUserAsync(UserDal user)
    {
        if (!_dbContext.Users.Contains(user))
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        return user.Id;
    }

    public async Task<UserDal> GetUserByIdAsync(Guid id)
    {
        return _dbContext.Users
            .Include(u => u.Friends)
            .First(u => u.Id == id);
    }

    public async Task<string[]> GetUserNamesByIdsAsync(Guid[] ids)
    {
        return _dbContext.Users
            .Where(u => ids.Contains(u.Id))
            .Select(u => u.Username)
            .ToArray();
    }

    public async Task<UserDal> GetUserByUsernameAsync(string username)
    {
        var user = _dbContext.Users
            .Include(u => u.Friends)
            .First(u => u.Username == username);
        return user;
    }

    public async Task<ICollection<UserDal>> GetUserFriendsAsync(Guid userId)
    {
        var user = _dbContext.Users
            .Include(u => u.Friends)
            .First(u => u.Id == userId);
        return user.Friends;
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return;
        }

        _dbContext.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<UserDal> UpdateUser(UserDal updatedModel)
    {
        var user = _dbContext.Users.First(u => u.Id == updatedModel.Id);

        user.AvatarUrl = updatedModel.AvatarUrl;
        user.Username = updatedModel.Username;
        user.Description = updatedModel.Description;
        user.Email = updatedModel.Email;
        return user;
    }

    public async Task MakeUsersFriendsAsync(Guid userId1, Guid userId2)
    {
        var user1 = _dbContext.Users.Include(u => u.Friends).First(u => u.Id == userId1);
        var user2 = _dbContext.Users.Include(u => u.Friends).First(u => u.Id == userId2);

        user1.Friends.Add(user2);
        user1.Friends.Add(user1);
        await _dbContext.SaveChangesAsync();
    }
}