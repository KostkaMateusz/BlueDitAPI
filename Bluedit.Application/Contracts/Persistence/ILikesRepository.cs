using Bluedit.Domain.Entities;

namespace Bluedit.Application.Contracts.Persistence;

public interface ILikesRepository<T> where T : LikeBase, new()
{
    Task AddLikeAsync(T like);
    Task<bool> CheckIfLikeExistAsync(Guid UserId, Guid parentId);
    void DeleteLike(T like);
    void DeleteLikes(IEnumerable<T> likes);
    Task<IEnumerable<T>> GetLikesByParentIdAsync(Guid parentId);
    Task<IEnumerable<T>> GetLikesByUserIdAsync(Guid UserId);
    Task<int> GetLikesCountByParentIdAsync(Guid parentId);
    Task<bool> SaveChangesAsync();
}
