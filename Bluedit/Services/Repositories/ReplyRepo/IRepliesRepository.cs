using Bluedit.Domain.Entities.ReplyEntities;

namespace Bluedit.Services.Repositories.ReplyRepo
{
    public interface IRepliesRepository
    {
        Task Addreplay(ReplyBase replay);
        Task DeleteReplayTree(ReplyBase replayRoot);
        Task<ReplyBase?> GetReplayById(Guid ReplayId);
        Task<ReplyBase?> GetReplayByParentId(Guid ParentId);
        Task<IEnumerable<Reply?>> GetRepliesByParentPostId(Guid ParentId);
        Task<IEnumerable<SubReplay>> GetSubRepliesByParentReplayId(Guid ParentId);
        Task<SubReplay?> GetSubReplyById(Guid SubreplayId);
        Task<bool> SaveChangesAsync();
        void UpdateReply(ReplyBase reply);
    }
}