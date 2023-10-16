using Bluedit.Entities;

namespace Bluedit.Services.Repositories
{
    public interface IRepliesRepository
    {
        Task Addreplay(ReplayBase replay);
        Task<ReplayBase?> GetReplayById(Guid ReplayId);
        Task<ReplayBase?> GetReplayByParentId(Guid ParentId);
        Task<ReplayBase?> GetReplayByParentPostId(Guid ParentId);
        Task<ReplayBase?> GetReplayByParentReplayId(Guid ParentId);
        Task<bool> SaveChangesAsync();
    }
}