using Bluedit.Application.DataModels.TopicDtos;
using Bluedit.Domain.Entities;

namespace Bluedit.Application.Contracts;

public interface ITopicRepository
{
    Task CreateTopicAsync(Topic topic);
    void DeleteTopicAsync(Topic topic);
    Task<int> GetTopicPostsCountAsync(string topicName);
    Task<Topic?> GetTopicWithNameAsync(string topicName);
    Task<bool> SaveChangesAsync();
    Task<bool> IsTopicExistAsync(string topicName);
    void UpdateTopicAsync(Topic topic);
    void IncrementPostCount(Topic topic);
    void DecrementPostCount(Topic topic);
    Task<IPagedList> GetAllTopicAsync(TopicResourceParameters topicResourceParameters);
}