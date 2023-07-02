using Bluedit.Entities;

namespace Bluedit.Services.Repositories
{
    public interface IPostRepository
    {
        Task AddPost(Post post);
        Task<Post?> GetPostByIdAsync(Guid postId);
        Task LoadPostUser(Post post);
        Task<bool> SaveChangesAsync();
    }
}