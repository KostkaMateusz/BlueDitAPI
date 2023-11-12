﻿using System.ComponentModel.DataAnnotations;

namespace Bluedit.Models.DataModels.ReplayDtos;

public class CreateReplyDto
{
    [Required]
    [MaxLength(100)]
    [MinLength(1)]
    public string Description { get; set; } = string.Empty;
}