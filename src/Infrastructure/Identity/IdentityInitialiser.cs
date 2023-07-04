using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PolizaWebAPI.Application.Common.Interfaces;

namespace PolizaWebAPI.Infrastructure.Identity;
public class IdentityInitialiser
{
    private readonly ILoggerManager _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityService _identityService;

    public IdentityInitialiser(ILoggerManager logger, UserManager<ApplicationUser> userManager, IIdentityService identityService)
    {
        _logger = logger;
        _userManager = userManager;
        _identityService = identityService;
    }



    public async Task Inicializar()
    {
        try
        {
            await GenerarUsuario();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al inicializar la base de datos.");
            throw;
        }
    }

    public async Task GenerarUsuario()
    {
      

        // Default users
        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _identityService.CreateUserAsync(administrator.UserName, "Password@123");
            
        }
    }
}