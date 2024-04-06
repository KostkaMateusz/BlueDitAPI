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

    public async Task Addreply(ReplyBase replay)
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

    public async Task<SubReplay?> GetSubReplyById(Guid subreplyId)
    {
        var replayReplay = await _dbContext.Replies.OfType<SubReplay>().FirstOrDefaultAsync(r => r.ReplyId == subreplyId);

        return replayReplay;
    }

    public async Task<ReplyBase?> GetReplayByParentId(Guid parentId)
    {
        var postReplay = await _dbContext.Replies.OfType<Reply>().FirstOrDefaultAsync(r => r.ParentPostId == parentId);
        if (postReplay is not null)
            return postReplay;

        var replayReplay = await _dbContext.Replies.OfType<SubReplay>().FirstOrDefaultAsync(r => r.ParentReplyId == parentId);
        if (replayReplay is not null)
            return replayReplay;

        return null;
    }

    public void UpdateReply(ReplyBase reply)
    {
        _dbContext.Update(reply);
    }

    public async Task DeleteReplyTree(ReplyBase replayRoot)
    {
        _dbContext.Replies.Remove(replayRoot);

        var childrenReposnes = await GetAllChildReply(replayRoot.ReplyId);

        _dbContext.RemoveRange(childrenReposnes);
    }


    public async Task<bool> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync() >= 0;
    }

    private async Task<IEnumerable<ReplyBase>> GetAllChildReply(Guid parentReplyId)
    {
        var firstChildrens = await _dbContext.Replies.OfType<SubReplay>().Where(reply => reply.ParentReplyId == parentReplyId).ToListAsync();

        // init output list
        List<ReplyBase> childrenResponses = new(firstChildrens);
        // init stack with first level children
        Stack<ReplyBase> reponses = new(firstChildrens);

        // until all reply sub tree is found
        while (reponses.IsNullOrEmpty() is false)
        {
            // Return one element from Stack
            var currentResponse = reponses.Pop();
            // Query DB for imediate Children of this Node
            var nodeReponses = await _dbContext.Replies.OfType<SubReplay>().Where(reply => reply.ParentReplyId == currentResponse.ReplyId).ToListAsync();
            // save children to list, push new children to be queried to stack
            foreach (var nodeReponse in nodeReponses)
            {
                reponses.Append(nodeReponse);
                childrenResponses.Add(nodeReponse);
            }
        }

        return childrenResponses;
    }

}