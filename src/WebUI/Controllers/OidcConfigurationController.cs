using Microsoft.AspNetCore.Mvc;

namespace PolizaWebAPI.WebUI.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class OidcConfigurationController : Controller
{
    private readonly ILogger<OidcConfigurationController> _logger;

    public OidcConfigurationController(
        ILogger<OidcConfigurationController> logger)
    {
        
        _logger = logger;
    }

}
