using Bluedit.Application.Contracts;
using Bluedit.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bluedit.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly BlueditDbContext _dbContext;

    public UserRepository(BlueditDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetUserByMail(string userMail)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == userMail);
    }

    public async Task<User?> GetUserById(Guid userId)
    {
        return await _dbContext.Users.FirstAsync(user => user.UserId == userId);
    }

    public async Task AddNewUser(User newUser)
    {
        await _dbContext.Users.AddAsync(newUser);
    }

    public async Task<User> GetUserAndAllRelatedEntities(Guid userId)
    {
        return await _dbContext.Users.FirstAsync(user => user.UserId == userId);
    }

    public void DeleteUser(User user)
    {
        _dbContext.Users.Remove(user);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync() >= 0;
    }
}