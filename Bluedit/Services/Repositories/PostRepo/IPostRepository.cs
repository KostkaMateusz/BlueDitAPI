using Bluedit.Domain.Entities;

namespace Bluedit.Services.Repositories.PostRepo;

public interface IPostRepository
{
    Task AddPost(Post post);
    Task<IEnumerable<Post?>?> GetAllPostsByTopicAsync(string topic);
    Task<Post?> GetPostByIdAsync(Guid postId);
    Task LoadPostUser(Post post);
    Task<bool> SaveChangesAsync();
    void UpdatePost(Post post);
    Task<bool> PostWithGivenIdExist(Guid postId);
}