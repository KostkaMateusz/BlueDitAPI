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

    public async Task<IEnumerable<Post?>?> GetAllPostsByTopicAsync(string topic)
    {
        return await _dbContext.Posts.Include(p => p.User).Where(p => p.TopicName == topic).ToListAsync();
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

    public void UpdatePost(Post post)
    {
        if(post is null)
            throw new ArgumentNullException(nameof(post));

        _dbContext.Posts.Update(post);
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
