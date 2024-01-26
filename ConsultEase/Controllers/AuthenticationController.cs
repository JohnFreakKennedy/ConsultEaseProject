using System.Net;
using AutoMapper;
using ConsultEaseAPI.Models;
using ConsultEaseBLL.DTOs.Authentication;
using ConsultEaseBLL.Helpers;
using ConsultEaseBLL.Interfaces;
using ConsultEaseBLL.Interfaces.Authentication;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ConsultEaseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AuthenticationController: ControllerBase
{
    private readonly JwtSettings _jwtSettings;
    private readonly IAuthService _authService;
    private readonly IRoleService _roleService;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public AuthenticationController(IAuthService authService, IOptionsSnapshot<JwtSettings> jwtSettings, IRoleService roleService, IUserService userService, ITokenService tokenService)
    {
        _authService = authService;
        _jwtSettings = jwtSettings.Value;
        _roleService = roleService;
        _userService = userService;
        _tokenService = tokenService;
    }
    
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(UserSignUpDto userDto)
    {
        var userId = await _authService.SignUpAsync(userDto);
        return Ok(userId);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(UserSignInDto userDto)
    {
        var user = await _authService.SignInAsync(userDto);
        var roles = await _roleService.GetRolesAsync(user);
        var accessToken = _tokenService.GenerateJwtToken(user, roles, _jwtSettings);
        var refreshToken = _tokenService.GenerateRefreshToken(_jwtSettings);
        
        SetRefreshToken(refreshToken.Token);
        SetAccessToken(accessToken);
        
        var result = await _tokenService.UpdateUserRefreshTokenAsync(user, refreshToken);
        return Ok(new Tokens(){AccessToken = accessToken, RefreshToken = refreshToken.Token});
    }
    
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var user = _userService.GetUserById(HttpContext.User);
        
        if(!user.RefreshToken.Equals(refreshToken))
            return Unauthorized("Invalid refresh token!");
        if(user.TokenExpires < DateTime.Now)
            return Unauthorized("Token has expired!");
        
        var roles = await _roleService.GetRolesAsync(user);
        
        string jwtToken = _tokenService.GenerateJwtToken(user, roles, _jwtSettings);
        var newRefreshToken = _tokenService.GenerateRefreshToken(_jwtSettings);
        
        SetRefreshToken(newRefreshToken.Token);
        SetAccessToken(jwtToken);
        
        var result = await _tokenService.UpdateUserRefreshTokenAsync(user, newRefreshToken);

        return Ok(jwtToken);
    }
    
    private void SetRefreshToken(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.Now.AddDays(7)
        };
        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }
    
    private void SetAccessToken(string accessToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.Now.AddHours(_jwtSettings.ExpirationInHours)
        };
        Response.Cookies.Append("accessToken", accessToken, cookieOptions);
    }

    private void SetAccessTokenNull()
    {
        var cookieOptions = new CookieOptions
        {
           MaxAge = TimeSpan.FromMilliseconds(1),
           SameSite = SameSiteMode.None,
           Secure = true
        };
        
        Response.Cookies.Append("accessToken", "", cookieOptions);
    }
    
    private void SetRefreshTokenNull()
    {
        var cookieOptions = new CookieOptions
        {
            MaxAge = TimeSpan.FromMilliseconds(1),
            SameSite = SameSiteMode.None,
            Secure = true
        };
        Response.Cookies.Append("refreshToken", "", cookieOptions);
    }
}


