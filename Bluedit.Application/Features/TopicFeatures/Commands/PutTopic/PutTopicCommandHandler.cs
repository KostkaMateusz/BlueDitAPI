using Bluedit.Application.Contracts;
using MediatR;

namespace Bluedit.Application.Features.TopicFeatures.Commands.PutTopic;

internal class PutTopicCommandHandler : IRequestHandler<PutTopicCommand, bool>
{
    private readonly ITopicRepository _topicRepository;

    public PutTopicCommandHandler(ITopicRepository topicRepository)
    {
        _topicRepository = topicRepository ?? throw new ArgumentNullException(nameof(topicRepository));
    }
    public async Task<bool> Handle(PutTopicCommand request, CancellationToken cancellationToken)
    {
        var topicForUpdate = await _topicRepository.GetTopicWithNameAsync(request.TopicName);

        if (topicForUpdate is null)
            return false;

        topicForUpdate.TopicDescription = request.TopicDescription;

        _topicRepository.UpdateTopicAsync(topicForUpdate);

        await _topicRepository.SaveChangesAsync();

        return true;
    }
}
