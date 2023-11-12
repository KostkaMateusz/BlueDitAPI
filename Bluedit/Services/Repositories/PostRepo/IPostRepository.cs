using Bluedit.Domain.Entities;

namespace Bluedit.Services.Repositories.PostRepo;

public interface IPostRepository
{
    Task AddPostAsync(Post post);
    Task<IEnumerable<Post?>?> GetAllPostsByTopicAsync(string topic);
    Task<Post?> GetPostByIdAsync(Guid postId);
    Task LoadPostUserAsync(Post post);
    Task<bool> SaveChangesAsync();
    void UpdatePost(Post post);
    Task<bool> PostWithGivenIdExistAsync(Guid postId);
    void DeletePost(Post post);
    Task<Post> LoadPostRepliesAsync(Post post);
}