﻿using Bluedit.Services.Repositories.TopicRepo;
using MediatR;

namespace Bluedit.Application.Features.TopicFeatures.Commands.PatchTopic;

public class PutTopicCommandHandler : IRequestHandler<PutTopicCommand, bool>
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

        _topicRepository.UpdateTopicAync(topicForUpdate);

        await _topicRepository.SaveChangesAsync();

        return true;
    }
}