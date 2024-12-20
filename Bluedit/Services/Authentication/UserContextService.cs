﻿using System.Security.Claims;

namespace Bluedit.Services.Authentication;

public interface IUserContextService
{
    ClaimsPrincipal User { get; }
    Guid GetUserId { get; }
}

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ClaimsPrincipal User => _httpContextAccessor.HttpContext!.User;

    public Guid GetUserId
    {
        get
        {
            var userClaim = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);

            return Guid.Parse(userClaim!.Value);
        }
    }
}