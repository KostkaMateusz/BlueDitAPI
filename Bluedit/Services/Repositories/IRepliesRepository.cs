using Bluedit.Entities;

namespace Bluedit.Services.Repositories
{
    public interface IRepliesRepository
    {
        Task Addreplay(ReplayBase replay);
        Task DeleteReplayTree(ReplayBase replayRoot);
        Task<ReplayBase?> GetReplayById(Guid ReplayId);
        Task<ReplayBase?> GetReplayByParentId(Guid ParentId);
        Task<IEnumerable<Reply?>> GetRepliesByParentPostId(Guid ParentId);
        Task<IEnumerable<SubReplay>> GetSubRepliesByParentReplayId(Guid ParentId);
        Task<SubReplay?> GetSubReplyById(Guid SubreplayId);
        Task<bool> SaveChangesAsync();
    }
}