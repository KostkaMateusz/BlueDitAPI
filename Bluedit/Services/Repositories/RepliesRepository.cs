using Bluedit.Entities;
using Microsoft.EntityFrameworkCore;

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
        _dbContext.Replies.Add(replay);
    }

    public async Task<IEnumerable<Reply?>> GetRepliesByParentPostId(Guid ParentId)
    {
        var postReplay = await _dbContext.Replies.OfType<Reply>().Where(r => r.ParentPostId == ParentId).ToListAsync();

        return postReplay;
    }

    public async Task<IEnumerable<SubReplay>?> GetSubRepliesByParentReplayId(Guid ParentId)
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


    public async Task<bool> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync() >= 0;
    }

}
