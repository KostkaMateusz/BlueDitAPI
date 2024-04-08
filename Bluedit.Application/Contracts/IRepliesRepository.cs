using Bluedit.Domain.Entities.ReplyEntities;

namespace Bluedit.Application.Contracts ;

public interface IRepliesRepository
{
    Task AddReply(ReplyBase replay);
    Task DeleteReplyTree(ReplyBase replayRoot);
    Task<ReplyBase?> GetReplyById(Guid replayId);
    Task<ReplyBase?> GetReplayByParentId(Guid parentId);
    Task<IEnumerable<Reply?>> GetRepliesByParentPostId(Guid parentId);
    Task<IEnumerable<SubReplay>> GetSubRepliesByParentReplyId(Guid parentId);
    Task<SubReplay?> GetSubReplyById(Guid subReplyId);
    Task<bool> SaveChangesAsync();
    void UpdateReply(ReplyBase reply);
}