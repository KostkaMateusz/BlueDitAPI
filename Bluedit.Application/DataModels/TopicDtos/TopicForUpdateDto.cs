﻿using System.ComponentModel.DataAnnotations;

namespace Bluedit.Application.DataModels.TopicDtos;

public class TopicForUpdateDto
{
    [MaxLength(60)] [MinLength(6)] public required string TopicDescription { get; set; }
}