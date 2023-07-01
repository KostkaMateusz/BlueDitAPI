using Bluedit.Entities;
using Bluedit.Helpers.Pagination;
using Bluedit.Models.DataModels.TopicDtos;
using Bluedit.ResourceParameters;

namespace Bluedit.Services.Repositories
{
    public interface ITopicRepository
    {
        Task CreateTopicAync(Topic topic);
        void DeleteTopicAsync(Topic topic);
        Task<int> GetTopicPostsCountAsync(string topicName);
        Task<Topic?> GetTopicWithNameAsync(string topicName);        
        Task<bool> SaveChangesAsync();
        Task<bool> IsTopicExistAsync(string topicName);
        Task<PagedList<Topic>> GetAllTopicAsync(TopicResourceParameters topicResourceParameters);
        Task UpdateTopicAync(Topic topic);
    }
}