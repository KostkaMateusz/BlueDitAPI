using Bluedit.Entities;
using Bluedit.Helpers.Pagination;
using Bluedit.ResourceParameters;
using Microsoft.EntityFrameworkCore;

namespace Bluedit.Services.Repositories;

public class TopicRepository : ITopicRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public TopicRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<bool> IsTopicExistAsync(string topicName)
    {
        var topicExist = await _applicationDbContext.Topics.AnyAsync(topic => topic.TopicName == topicName);

        return topicExist;
    }

    public async Task<Topic?> GetTopicWithNameAsync(string topicName)
    {
        var topic=await _applicationDbContext.Topics.FirstOrDefaultAsync(topic => topic.TopicName == topicName);

        return topic;
    }

    public async Task<int> GetTopicPostsCountAsync(string topicName)
    {
        var postInTopicCount = await _applicationDbContext.Posts.CountAsync(post=>post.TopicName==topicName);

        return postInTopicCount;
    }

    public async Task CreateTopicAync(Topic topic)
    {        
        if(topic is null)
            throw new ArgumentNullException(nameof(topic));       

        await _applicationDbContext.AddAsync(topic);
    }

    public void DeleteTopicAsync(Topic topic)
    {
        if (topic is null)
            throw new ArgumentNullException(nameof(topic));

        _applicationDbContext.Topics.Remove(topic);
    }

    public async Task<PagedList<Topic>> GetAllTopicAsync(TopicResourceParameters topicResourceParameters)
    {
        if (topicResourceParameters is null)
            throw new ArgumentNullException(nameof(topicResourceParameters));

        var topicCollection= _applicationDbContext.Topics as IQueryable<Topic>;

        //search by name
        if (string.IsNullOrWhiteSpace(topicResourceParameters.TopicName) is false)
        {
            var topicName=topicResourceParameters.TopicName.Trim().ToUpper();
            topicCollection = topicCollection.Where(topic => topic.TopicName == topicName);    
        }

        if(string.IsNullOrWhiteSpace(topicResourceParameters.SearchQuery) is false)
        {
            var searchQuery= topicResourceParameters.SearchQuery.Trim();
            topicCollection = topicCollection.Where(topic => topic.TopicName.Contains(searchQuery)
                                                            || topic.TopicDescription.Contains(searchQuery));
        }

        return await PagedList<Topic>.CreateAsync(topicCollection, topicResourceParameters.PageNumber, topicResourceParameters.PageSize);
    }


    public async Task<bool> SaveChangesAsync()
    {
        return (await _applicationDbContext.SaveChangesAsync()>=0);
    }
}
