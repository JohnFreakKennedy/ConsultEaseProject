using ConsultEaseBLL.Interfaces.Authentication;
using ConsultEaseDAL.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ConsultEaseBLL.Services.Authentication;

public class RoleService: IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;

    public RoleService(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }
    
    public async Task<IEnumerable<IdentityRole>> GetRolesAsync()
    {
        return await _roleManager.Roles.ToListAsync();
    }

    public async Task<IEnumerable<string>> GetRolesAsync(User user)
    {
        return (await _userManager.GetRolesAsync(user)).ToList();
    }

    public async Task CreateRoleAsync(string roleName)
    {
        var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
        if (!result.Succeeded) throw new Exception($"Role {roleName} creation failed!");
    }
}