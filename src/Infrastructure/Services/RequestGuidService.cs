using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolizaWebAPI.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace PolizaWebAPI.Infrastructure.Services;
public class RequestGuidService : IRequestGuid
{
    private const string RequestGuidKey = "RequestGuid";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public RequestGuidService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetCurrentGuid()
    {
        HttpContext? httpContext = _httpContextAccessor?.HttpContext;

        if (httpContext != null && !httpContext.Items.ContainsKey(RequestGuidKey))
        {
            Guid requestGuid = Guid.NewGuid();
            httpContext.Items[RequestGuidKey] = requestGuid;
            return requestGuid;
        }

        return httpContext?.Items[RequestGuidKey] as Guid? ?? Guid.Empty;
    }
}