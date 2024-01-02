﻿using Bluedit.Domain.Entities;
using Bluedit.Services.Repositories.TopicRepo;
using MediatR;

namespace Bluedit.Application.Features.TopicFeatures.Commands.CreateTopic;

public class CreateTopicCommandHandler : IRequestHandler<CreateTopicCommand, Topic>
{
    private readonly ITopicRepository _topicRepository;

    public CreateTopicCommandHandler(ITopicRepository topicRepository)
    {
        _topicRepository = topicRepository ?? throw new ArgumentNullException(nameof(topicRepository));
    }

    public async Task<Topic> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
    {
        var topicEntity = new Topic { TopicName = request.TopicName, TopicDescription = request.TopicDescription };

        await _topicRepository.CreateTopicAync(topicEntity);

        await _topicRepository.SaveChangesAsync();

        return topicEntity;
    }
}