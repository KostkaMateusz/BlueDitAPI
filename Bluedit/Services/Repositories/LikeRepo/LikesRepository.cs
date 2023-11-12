using Microsoft.EntityFrameworkCore;
using Bluedit.Domain.Entities.LikeEntities;

namespace Bluedit.Services.Repositories.LikeRepo;

public class LikesRepository<T> : ILikesRepository<T> where T : LikeBase, new()
{
    private readonly BlueditDbContext _dbContext;
    private readonly DbSet<T> _likeContex;

    public LikesRepository(BlueditDbContext dbContext)
    {
        _dbContext = dbContext;
        _likeContex = _dbContext.Set<T>();
    }

    public async Task AddLikeAsync(T like)
    {
        await _likeContex.AddAsync(like);
    }

    public async Task<IEnumerable<T>> GetLikesByParentIdAsync(Guid parentId)
    {
        return await _likeContex.Include(l => l.User).Where(l => l.ParentId == parentId).ToListAsync();
    }

    public async Task<IEnumerable<T>> GetLikesByUserIdAsync(Guid UserId)
    {
        return await _likeContex.Where(l => l.UserId == UserId).ToListAsync();
    }

    public async Task<int> GetLikesCountByParentIdAsync(Guid parentId)
    {
        return await _likeContex.Where(l => l.ParentId == parentId).CountAsync();
    }

    public async Task<bool> CheckIfLikeExistAsync(Guid UserId, Guid parentId)
    {
        return await _likeContex.Where(l => l.ParentId == parentId && l.UserId == UserId).AnyAsync();
    }

    public void DeleteLike(T like)
    {
        _likeContex.Remove(like);
    }

    public void DeleteLikes(IEnumerable<T> likes)
    {
        _likeContex.RemoveRange(likes);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync() >= 0;
    }
}