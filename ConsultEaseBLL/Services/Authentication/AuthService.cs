using ConsultEaseBLL.DTOs.Authentication;
using ConsultEaseBLL.Exceptions;
using ConsultEaseBLL.Helpers;
using ConsultEaseBLL.Interfaces;
using ConsultEaseBLL.Interfaces.Authentication;
using ConsultEaseDAL.Entities.Auth;
using Microsoft.AspNetCore.Identity;

namespace ConsultEaseBLL.Services.Authentication;

public class AuthService: IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IAppointmentService _appointmentService;
    private readonly JwtSettings _jwtSettings;

    public AuthService( UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
        SignInManager<User> signInManager, ITokenService tokenService, JwtSettings jwtSettings
        , IAppointmentService appointmentService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _appointmentService = appointmentService;
        _jwtSettings = jwtSettings;
    }
    
    public async Task<string> SignUpAsync(UserSignUpDto userDto)
    {
        var refreshToken = _tokenService.GenerateRefreshToken(_jwtSettings);
        
        var result = await _userManager.CreateAsync(new User
        {
            Id = Convert.ToInt32(Guid.NewGuid()),
            Email = userDto.Email,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            RefreshToken = refreshToken.Token,
            TokenCreated = refreshToken.Created,
            TokenExpires = refreshToken.Expires
        }, userDto.Password);
        
        if(!result.Succeeded) 
            throw new Exception(string.Join(';', result.Errors.Select(e => e.Description)));
        else
        {
            var userToCreate = await _userManager.FindByEmailAsync(userDto.Email);

            var toCreate = userToCreate ?? 
                           throw new UserNotFoundException("Can't find user with stated email");

            var roleResult = await _userManager.AddToRoleAsync(toCreate, userDto.Role);
            var roles = await _userManager.GetRolesAsync(toCreate);

            return userToCreate.Id.ToString();
        }
    }

    public async Task<User> SignInAsync(UserSignInDto userDto)
    {
        var user = _userManager.Users.SingleOrDefault(user => user.Email == userDto.Email);
        if (user == null) throw new UserNotFoundException($"User with email {userDto.Email} was not found!");
        
        return await _userManager.CheckPasswordAsync(user, userDto.Password) ? user : 
            throw new ArgumentException("Password is incorrect!");
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}