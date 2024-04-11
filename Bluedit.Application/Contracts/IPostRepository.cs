using Bluedit.Application.DataModels.PostDtos;
using Bluedit.Domain.Entities;

namespace Bluedit.Application.Contracts;

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
    Task LoadPostLikesAsync(Post post);
    Task<IPagedList> GetPostsAsync(PostResourceParameters postResourceParameters);
    Task<int> CountPostLikes(Guid parentId);
}