using Bluedit.Entities;

namespace Bluedit.Services.Repositories
{
    public interface IPostRepository
    {
        Task AddPost(Post post);
        Task<IEnumerable<Post?>?> GetAllPostsByTopicAsync(string topic);
        Task<Post?> GetPostByIdAsync(Guid postId);
        Task LoadPostUser(Post post);
        Task<bool> SaveChangesAsync();
        void UpdatePost(Post post);
    }
}