using System.Security.Claims;
using AutoMapper;
using ConsultEaseBLL.DTOs.Authentication;
using ConsultEaseBLL.Exceptions;
using ConsultEaseBLL.Interfaces.Authentication;
using ConsultEaseDAL.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ConsultEaseBLL.Services.Authentication;

public class UserService: IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    
    public UserService(UserManager<User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }
    
    public UserDto GetCurrentUser(ClaimsPrincipal userPrincipal)
    {
        var userId = _userManager.GetUserId(userPrincipal);
        var user = _userManager.Users.SingleOrDefault(user => user.Id.ToString() == userId);
        if(user == null) throw new UserNotFoundException("User was not found!");
        return _mapper.Map<UserDto>(user);
    }

    public User GetUserById(ClaimsPrincipal userPrincipal)
    {
        var userId = _userManager.GetUserId(userPrincipal);
        var user = _userManager.Users.SingleOrDefault(user => user.Id.ToString() == userId);
        if(user == null) throw new UserNotFoundException("User was not found!");
        return user;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public string GetUserId(ClaimsPrincipal userPrincipal) =>
        _userManager.GetUserId(userPrincipal) ?? throw new UserNotFoundException("User was not found!");
    
    public async Task<IdentityResult> DeleteUserAsync(ClaimsPrincipal userPrincipal)
    {
        var user = await _userManager.GetUserAsync(userPrincipal);
        if (user == null) throw new UserNotFoundException("User was not found!");
        return await _userManager.DeleteAsync(user);
    }
}