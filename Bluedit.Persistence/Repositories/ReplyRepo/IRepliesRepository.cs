using Bluedit.Domain.Entities.ReplyEntities;

namespace Bluedit.Persistence.Repositories.ReplyRepo
{
    public interface IRepliesRepository
    {
        Task Addreply(ReplyBase replay);
        Task DeleteReplyTree(ReplyBase replayRoot);
        Task<ReplyBase?> GetReplyById(Guid replayId);
        Task<ReplyBase?> GetReplayByParentId(Guid parentId);
        Task<IEnumerable<Reply?>> GetRepliesByParentPostId(Guid parentId);
        Task<IEnumerable<SubReplay>> GetSubRepliesByParentReplayId(Guid parentId);
        Task<SubReplay?> GetSubReplyById(Guid subreplyId);
        Task<bool> SaveChangesAsync();
        void UpdateReply(ReplyBase reply);
    }
}