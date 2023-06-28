﻿using System.Security.Claims;

namespace Bluedit.Services.UserAuthServices.Authentication;

public interface IUserContextService
{
    ClaimsPrincipal User { get; }
    Guid? GetUserId { get; }
}

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

    public Guid? GetUserId
    {
        get
        {
            var userClaim = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
            if (userClaim is null) return null;
            else
            {
                return Guid.Parse(userClaim.Value);
            }
        }
    }
}
