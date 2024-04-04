using Bluedit.Application.Contracts;
using MediatR;

namespace Bluedit.Application.Features.TopicFeatures.Commands.DeleteTopic;

public class DeleteTopicCommandHandler : IRequestHandler<DeleteTopicCommand, bool>
{
    private readonly ITopicRepository _topicRepository;

    public DeleteTopicCommandHandler(ITopicRepository topicRepository)
    {
        _topicRepository = topicRepository ?? throw new ArgumentNullException(nameof(topicRepository));
    }

    public async Task<bool> Handle(DeleteTopicCommand request, CancellationToken cancellationToken)
    {
        var topicToDelete = await _topicRepository.GetTopicWithNameAsync(request.TopicName);

        if (topicToDelete is null)
            return false;

        _topicRepository.DeleteTopicAsync(topicToDelete);
        await _topicRepository.SaveChangesAsync();

        return true;
    }
}
