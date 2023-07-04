using PolizaWebAPI.Application.Common.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Models;

public class User
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
}