using Bluedit.Entities;
using Bluedit.Helpers.Pagination;
using Bluedit.ResourceParameters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

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

        _applicationDbContext.Entry(topic).Collection(p => p.Posts).Load();

        _applicationDbContext.Topics.Remove(topic);
    }

    public async Task UpdateTopicAync (Topic topic)
    {
        _applicationDbContext.Update(topic);
    }
    public async Task<PagedList<Topic>> GetAllTopicAsync(TopicResourceParameters topicResourceParameters)
    {
        if (topicResourceParameters is null)
            throw new ArgumentNullException(nameof(topicResourceParameters));

        var topicCollectionQuery= _applicationDbContext.Topics as IQueryable<Topic>;

        // filter by name
        if (string.IsNullOrWhiteSpace(topicResourceParameters.TopicName) is false)
        {
            var topicName=topicResourceParameters.TopicName.Trim().ToUpper();
            topicCollectionQuery = topicCollectionQuery.Where(topic => topic.TopicName == topicName);    
        }

        //search by in name and description
        if(string.IsNullOrWhiteSpace(topicResourceParameters.SearchQuery) is false)
        {
            var searchQuery= topicResourceParameters.SearchQuery.Trim();
            topicCollectionQuery = topicCollectionQuery.Where(topic => topic.TopicName.Contains(searchQuery)
                                                            || topic.TopicDescription.Contains(searchQuery));
        }
        topicCollectionQuery.OrderByDescending(t => t.PostCount);

        return await PagedList<Topic>.CreateAsync(topicCollectionQuery, topicResourceParameters.PageNumber, topicResourceParameters.PageSize);
    }


    public async Task IncrementPostCount(Topic topic)
    {
        if (topic is null)
            throw new ArgumentNullException(nameof(topic));

        topic.PostCount++;

       _applicationDbContext.Update(topic);
    }


    public async Task DecrementPostCount(Topic topic)
    {
        if (topic is null)
            throw new ArgumentNullException(nameof(topic));

        topic.PostCount--;

        _applicationDbContext.Update(topic);
    }



    public async Task<bool> SaveChangesAsync()
    {
        return (await _applicationDbContext.SaveChangesAsync()>=0);
    }
}
