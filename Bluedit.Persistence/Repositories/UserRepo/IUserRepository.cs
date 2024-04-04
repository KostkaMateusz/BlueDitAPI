using Bluedit.Domain.Entities;

namespace Bluedit.Persistence.Repositories.UserRepo;

public interface IUserRepository
{
    Task AddNewUser(User newUser);
    void DeleteUser(User user);
    Task<User> GetUserAndAllRelatedEntities(Guid userId);
    Task<User?> GetUserById(Guid userId);
    Task<User?> GetUserByMail(string userMail);
    Task<bool> SaveChangesAsync();
}