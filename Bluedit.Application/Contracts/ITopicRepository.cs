using Bluedit.Application.DataModels.TopicDtos;
using Bluedit.Domain.Entities;
using Bluedit.Models.DataModels.TopicDtos;

namespace Bluedit.Services.Repositories.TopicRepo;

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
    Task<IPagedList<Topic>> GetAllTopicAsync(TopicResourceParametersBase topicResourceParametersBase);
}