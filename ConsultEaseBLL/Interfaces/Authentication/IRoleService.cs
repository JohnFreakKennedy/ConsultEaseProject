using Microsoft.AspNetCore.Identity;
using ConsultEaseDAL.Entities.Auth;

namespace ConsultEaseBLL.Interfaces.Authentication;

public interface IRoleService
{
    Task<IEnumerable<IdentityRole>> GetRolesAsync();
    Task<IEnumerable<string>> GetRolesAsync(User user);
    Task CreateRoleAsync(string role);
}