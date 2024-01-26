using AutoMapper;
using ConsultEaseBLL.DTOs.Authentication;
using ConsultEaseBLL.Interfaces.Authentication;
using ConsultEaseDAL.Entities.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ConsultEaseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController: ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService) => _userService = userService;
    
    [HttpGet("current")]
    public IActionResult GetCurrentUser()
    {
        var user = _userService.GetCurrentUser(User);
        return Ok(user);
    }
    
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }
    
    [HttpGet("id")]
    public IActionResult GetUserId()
    {
        var userId = _userService.GetUserId(User);
        return Ok(userId);
    }
}