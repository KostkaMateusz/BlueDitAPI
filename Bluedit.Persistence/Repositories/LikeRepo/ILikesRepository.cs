using Bluedit.Domain.Entities.LikeEntities;

namespace Bluedit.Persistence.Repositories.LikeRepo;

public interface ILikesRepository<T> where T : LikeBase, new()
{
    Task<T?> GetLike(Guid parentId, Guid userId);
    Task AddLikeAsync(T like);
    Task<bool> CheckIfLikeExistAsync(Guid userId, Guid parentId);
    void DeleteLike(T like);
    void DeleteLikes(IEnumerable<T> likes);
    Task<IEnumerable<T>> GetLikesByParentIdAsync(Guid parentId);
    Task<IEnumerable<T>> GetLikesByUserIdAsync(Guid userId);
    Task<int> GetLikesCountByParentIdAsync(Guid parentId);
    Task<bool> SaveChangesAsync();
}