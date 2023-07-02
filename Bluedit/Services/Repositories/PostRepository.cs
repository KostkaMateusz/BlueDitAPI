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

    public async Task<Post?> GetPostByIdAsync(Guid postId)
    {
        return await _dbContext.Posts.FirstOrDefaultAsync(post=>post.PostId==postId);
    }

    public async Task LoadPostUser(Post post)
    {
        if(post is null)
            throw new ArgumentNullException(nameof(post));

        await _dbContext.Entry(post).Reference(post => post.User).LoadAsync();
    }

    public async Task AddPost(Post post)
    {
        await _dbContext.Posts.AddAsync(post);
    }

    public async Task DeletePost(Post post)
    {
        if(post is null)
            throw new ArgumentNullException(nameof(post));

        _dbContext.Posts.Remove(post);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync() >= 0;
    }

}
