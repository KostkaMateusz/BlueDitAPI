using Bluedit.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bluedit.Services.Repositories;

public class PostRepository : IPostRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PostRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddPost(Post post)
    {
        await _dbContext.Posts.AddAsync(post);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync() >= 0;
    }

}
