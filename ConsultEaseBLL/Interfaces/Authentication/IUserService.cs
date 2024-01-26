using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using ConsultEaseDAL.Entities.Auth;
using ConsultEaseBLL.DTOs.Authentication;

namespace ConsultEaseBLL.Interfaces.Authentication;

public interface IUserService
{
    UserDto GetCurrentUser(ClaimsPrincipal userPrincipal);
    User GetUserById(ClaimsPrincipal userPrincipal);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    string GetUserId(ClaimsPrincipal user);
    Task<IdentityResult> DeleteUserAsync(ClaimsPrincipal user);
}