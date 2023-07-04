using System.Security.Claims;

using PolizaWebAPI.Application.Common.Interfaces;

namespace PolizaWebAPI.WebUI.Services;

public class CurrentTransactionService : ICurrentTransactionService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRequestGuid _requestGuid;
    public CurrentTransactionService(IHttpContextAccessor httpContextAccessor, IRequestGuid requestGuid) 
    {
        _httpContextAccessor = httpContextAccessor;
        _requestGuid = requestGuid;
    }

    public string? Transaction => _httpContextAccessor?.HttpContext?.Items["RequestGuid"]?.ToString() ?? _requestGuid.GetCurrentGuid().ToString();
}
