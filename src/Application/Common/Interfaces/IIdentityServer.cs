using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolizaWebAPI.Application.Common.Models;

namespace PolizaWebAPI.Application.Common.Interfaces;
public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string username);
    Task<bool> CheckPasswordAsync(string username, string password);
    Task<bool> IsInRoleAsync(string userId, string role);
    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(Result Result, Guid UserId)> CreateUserAsync(string userName, string password);

    Task<Result> DeleteUserAsync(string userId);
}