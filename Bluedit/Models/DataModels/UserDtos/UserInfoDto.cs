﻿namespace Bluedit.Models.DataModels.UserDtos;

public class UserInfoDto
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}
