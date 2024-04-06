using Bluedit.Application.Contracts;
using Bluedit.Application.DataModels.PostDtos;
using Bluedit.Domain.Entities;
using Bluedit.Persistence.Helpers.Pagination;
using Bluedit.Persistence.Helpers.Sorting;
using Microsoft.EntityFrameworkCore;

namespace Bluedit.Persistence.Repositories.PostRepo;

public class PostRepository : IPostRepository
{
    private readonly BlueditDbContext _dbContext;
    private readonly IPropertyMappingService _propertyMappingService;
    public PostRepository(BlueditDbContext dbContext,IPropertyMappingService propertyMappingService)
    {
        _dbContext = dbContext;
        _propertyMappingService =
            propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
    }

    public async Task<IEnumerable<Post?>?> GetAllPostsByTopicAsync(string topic)
    {
        return await _dbContext.Posts.Include(p => p.User).Where(p => p.TopicName == topic).ToListAsync();
    }

    public async Task<IPagedList<Post>> GetPostsAsync(PostResourceParameters postResourceParameters)
    {
        if (postResourceParameters is null)
            throw new ArgumentNullException(nameof(postResourceParameters));

        var postCollectionQuery = _dbContext.Posts.Include(p => p.User) as IQueryable<Post>;
        
        // filter by TopicName
        if (string.IsNullOrWhiteSpace(postResourceParameters.TopicName) is false)
        {
            var topicName = postResourceParameters.TopicName.Trim().ToUpper();
            postCollectionQuery = postCollectionQuery.Where(post => post.TopicName == topicName);
        }
        //filter by UserName
        if (string.IsNullOrWhiteSpace(postResourceParameters.UserName) is false)
        {
            var userName = postResourceParameters.UserName.Trim().ToUpper();
            postCollectionQuery = postCollectionQuery.Where(post => post.User.Name == userName);
        }
        //search by in title and description
        if (string.IsNullOrWhiteSpace(postResourceParameters.SearchQuery) is false)
        {
            var searchQuery = postResourceParameters.SearchQuery.Trim();
            postCollectionQuery = postCollectionQuery.Where(post => post.Title.Contains(searchQuery)
                                                                     || post.Description.Contains(searchQuery));
        }
        //apply sorting
        if (string.IsNullOrWhiteSpace(postResourceParameters.OrderBy) is false)
        {
            // get property mapping dictionary
            var postPropertyMappingDictionary = _propertyMappingService.GetPropertyMapping<PostInfoDto, Post>();

            postCollectionQuery =
                postCollectionQuery.ApplySort(postResourceParameters.OrderBy, postPropertyMappingDictionary);
        }
        return await PagedList<Post>.CreateAsync(postCollectionQuery, postResourceParameters.PageNumber,
            postResourceParameters.PageSize);
    }
    
    
    public async Task<bool> PostWithGivenIdExistAsync(Guid postId)
    {
        return await _dbContext.Posts.AnyAsync(post => post.PostId == postId);
    }

    public async Task<Post> LoadPostRepliesAsync(Post post)
    {
        await _dbContext.Entry(post).Collection(p=>p.Reply).LoadAsync();

        return post;
    }

    public async Task<Post?> GetPostByIdAsync(Guid postId)
    {
        return await _dbContext.Posts.FirstOrDefaultAsync(post => post.PostId == postId);
    }

    public async Task LoadPostUserAsync(Post post)
    {
        if (post is null)
            throw new ArgumentNullException(nameof(post));

        await _dbContext.Entry(post).Reference(postClass => postClass.User).LoadAsync();
    }

    public async Task LoadPostLikesAsync(Post post)
    {
        if (post is null)
            throw new ArgumentNullException(nameof(post));

        await _dbContext.Entry(post).Reference(post => post.PostLikes).LoadAsync();
    }

    public void UpdatePost(Post post)
    {
        if (post is null)
            throw new ArgumentNullException(nameof(post));

        _dbContext.Posts.Update(post);
    }

    public async Task AddPostAsync(Post post)
    {
        await _dbContext.Posts.AddAsync(post);
    }

    public void DeletePost(Post post)
    {        
        if (post is null)
            throw new ArgumentNullException(nameof(post));

        _dbContext.Posts.Remove(post);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync() >= 0;
    }

}
