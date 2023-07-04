using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PolizaWebAPI.Infrastructure.Identity;
using PolizaWebAPI.WebUI.Controllers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using WebUI.Models;
using PolizaWebAPI.Application.Common.Exceptions;
using PolizaWebAPI.Application.Common.Security;
using PolizaWebAPI.Application.Common.Interfaces;
using MediatR;

namespace WebUI.Controllers;

public class AuthController : ApiControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IIdentityService _identityService;
    public AuthController(SignInManager<ApplicationUser> signInManager, IIdentityService identityService)
    {
        this._signInManager = signInManager;
        this._identityService = identityService;
    }
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Login([FromBody] LoginUserModel request)
    {
        if (ModelState.IsValid)
        {
            var user = await _identityService.GetUserNameAsync(request.Username);
            if (user != null && await _identityService.CheckPasswordAsync(user, request.Password))
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.ToString()),
                };

                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JwT_Secret"].ToString()));
                var token = new JwtSecurityToken(
                    issuer: Configuration["ApplicationSettings:Client_URL"].ToString(),
                    audience: Configuration["ApplicationSettings:Client_URL"].ToString(),
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(24),
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    ExpirationToken = token.ValidTo
                });
            }
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
                Detail = "Usuario y/o contraseña incorrectos"
            };
          
            return Unauthorized(details);
        }

        return Unauthorized(ModelState);
    }
    
    [HttpPost("[action]")]
    [ProducesResponseType(typeof(Unit), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create(User user)
    {
        if (ModelState.IsValid)
        {
            var result = await _identityService.CreateUserAsync(user.UserName, user.Password);
            if (result.Result.Succeeded)
                return Ok("Usuario creado exitosamente");
            else
                return Ok(result.Result.Errors);
        }

        var details = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "BadRequest",
            Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
        };
        return BadRequest(details);
    }

}