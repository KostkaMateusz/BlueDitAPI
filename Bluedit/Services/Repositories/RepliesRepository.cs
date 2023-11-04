using Bluedit.Entities;
using Bluedit.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Bluedit.Services.Repositories;

public class RepliesRepository : IRepliesRepository
{
    private readonly ApplicationDbContext _dbContext;

    public RepliesRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ReplayBase?> GetReplayById(Guid ReplayId)
    {
        return await _dbContext.Replies.FirstOrDefaultAsync(r => r.ReplayBaseId == ReplayId); ;
    }

    public async Task Addreplay(ReplayBase replay)
    {
        await _dbContext.Replies.AddAsync(replay);
    }

    public async Task<IEnumerable<Reply?>> GetRepliesByParentPostId(Guid ParentId)
    {
        var postReplay = await _dbContext.Replies.OfType<Reply>().Where(r => r.ParentPostId == ParentId).ToListAsync();

        return postReplay;
    }

    public async Task<IEnumerable<SubReplay>> GetSubRepliesByParentReplayId(Guid ParentId)
    {
        var replayReplay = await _dbContext.Replies.OfType<SubReplay>().Where(r => r.ParentReplyId == ParentId).ToListAsync();

        return replayReplay;
    }

    public async Task<SubReplay?> GetSubReplyById(Guid SubreplayId)
    {
        var replayReplay = await _dbContext.Replies.OfType<SubReplay>().FirstOrDefaultAsync(r => r.ReplayBaseId == SubreplayId);

        return replayReplay;
    }

    public async Task<ReplayBase?> GetReplayByParentId(Guid ParentId)
    {
        var postReplay = await _dbContext.Replies.OfType<Reply>().FirstOrDefaultAsync(r => r.ParentPostId == ParentId);
        if (postReplay is not null)
            return postReplay;

        var replayReplay = await _dbContext.Replies.OfType<SubReplay>().FirstOrDefaultAsync(r => r.ParentReplyId == ParentId);
        if (replayReplay is not null)
            return replayReplay;

        return null;
    }

    public void UpdateReply(ReplayBase reply)
    {
        _dbContext.Update(reply);
    }

    public async Task DeleteReplayTree(ReplayBase replayRoot)
    {
        _dbContext.Replies.Remove(replayRoot);

        var childrenReposnes = await GetAllChildReply(replayRoot.ReplayBaseId);
        
        _dbContext.RemoveRange(childrenReposnes);
    }


    public async Task<bool> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync() >= 0;
    }

    private async Task<IEnumerable<ReplayBase>> GetAllChildReply(Guid parentReplyId)
    {        
        var firstChildrens = await _dbContext.Replies.OfType<SubReplay>().Where(reply => reply.ParentReplyId == parentReplyId).ToListAsync();

        // init output list
        List<ReplayBase> childrenResponses = new(firstChildrens);
        // init stack with first level children
        Stack<ReplayBase> reponses = new(firstChildrens);

        // until all reply sub tree is found
        while (reponses.IsNullOrEmpty() is false)
        {
            // Return one element from Stack
            var currentResponse = reponses.Pop();
            // Query DB for imediate Children of this Node
            var nodeReponses = await _dbContext.Replies.OfType<SubReplay>().Where(reply => reply.ParentReplyId == currentResponse.ReplayBaseId).ToListAsync();
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
