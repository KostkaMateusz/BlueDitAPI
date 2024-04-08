using Bluedit.Domain.Entities.ReplyEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Bluedit.Persistence.Repositories.ReplyRepo;

public class RepliesRepository : IRepliesRepository
{
    private readonly BlueditDbContext _dbContext;

    public RepliesRepository(BlueditDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ReplyBase?> GetReplyById(Guid replayId)
    {
        return await _dbContext.Replies.FirstOrDefaultAsync(r => r.ReplyId == replayId); 
    }

    public async Task AddReply(ReplyBase replay)
    {
        await _dbContext.Replies.AddAsync(replay);
    }

    public async Task<IEnumerable<Reply?>> GetRepliesByParentPostId(Guid parentId)
    {
        var postReplay = await _dbContext.Replies.OfType<Reply>().Where(r => r.ParentPostId == parentId).ToListAsync();

        return postReplay;
    }

    public async Task<IEnumerable<SubReplay>> GetSubRepliesByParentReplayId(Guid parentId)
    {
        var replayReplay = await _dbContext.Replies.OfType<SubReplay>().Where(r => r.ParentReplyId == parentId).ToListAsync();

        return replayReplay;
    }

    public async Task<SubReplay?> GetSubReplyById(Guid subReplyId)
    {
        var replayReplay = await _dbContext.Replies.OfType<SubReplay>().FirstOrDefaultAsync(r => r.ReplyId == subReplyId);

        return replayReplay;
    }

    public async Task<ReplyBase?> GetReplayByParentId(Guid parentId)
    {
        var postReplay = await _dbContext.Replies.OfType<Reply>().FirstOrDefaultAsync(r => r.ParentPostId == parentId);
        if (postReplay is not null)
            return postReplay;

        var replayReplay = await _dbContext.Replies.OfType<SubReplay>().FirstOrDefaultAsync(r => r.ParentReplyId == parentId);
        
        return replayReplay;
    }

    public void UpdateReply(ReplyBase reply)
    {
        _dbContext.Update(reply);
    }

    public async Task DeleteReplyTree(ReplyBase replayRoot)
    {
        _dbContext.Replies.Remove(replayRoot);

        var childrenReposes = await GetAllChildReply(replayRoot.ReplyId);

        _dbContext.RemoveRange(childrenReposes);
    }


    public async Task<bool> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync() >= 0;
    }

    private async Task<IEnumerable<ReplyBase>> GetAllChildReply(Guid parentReplyId)
    {
        var firstChildren = await _dbContext.Replies.OfType<SubReplay>().Where(reply => reply.ParentReplyId == parentReplyId).ToListAsync();

        // init output list
        List<ReplyBase> childrenResponses = [..firstChildren];
        // init stack with first level children
        Stack<ReplyBase> responses = new(firstChildren);

        // until all reply sub tree is found
        while (responses.IsNullOrEmpty() is false)
        {
            // Return one element from Stack
            var currentResponse = responses.Pop();
            // Query DB for immediate Children of this Node
            var nodeResponses = await _dbContext.Replies.OfType<SubReplay>().Where(reply => reply.ParentReplyId == currentResponse.ReplyId).ToListAsync();
            // save children to list, push new children to be queried to stack
            foreach (var nodeResponse in nodeResponses)
            {
                responses.Push(nodeResponse);
                childrenResponses.Add(nodeResponse);
            }
        }

        return childrenResponses;
    }

}