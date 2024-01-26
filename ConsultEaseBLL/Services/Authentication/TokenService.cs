using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ConsultEaseBLL.DTOs.Authentication;
using ConsultEaseBLL.Exceptions;
using ConsultEaseBLL.Interfaces.Authentication;
using ConsultEaseBLL.Helpers;
using ConsultEaseDAL.Entities.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ConsultEaseBLL.Services.Authentication;

public class TokenService: ITokenService
{
    private readonly UserManager<User> _userManager;
    private List<Claim> SetUserClaims(User user)
    {
        var claims = new List<Claim>
        {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "Student"),
                new Claim(ClaimTypes.Role, "Professor")
        };
        return claims;
    }
    private IEnumerable<Claim> SetRoleClaims(IEnumerable<string> roles)
        => roles.Select(role => new Claim(ClaimTypes.Role, role));
    
    public TokenService(UserManager<User> userManager) => _userManager = userManager;
    
    
    public async Task<IdentityResult> UpdateUserRefreshTokenAsync(User user, RefreshToken token)
    {
        if (user == null) throw new UserNotFoundException("User was not found!");
        user.RefreshToken = token.Token;
        user.TokenCreated = token.Created;
        user.TokenExpires = token.Expires;
        var result = await _userManager.UpdateAsync(user);
        if(!result.Succeeded)
            throw new UserTokenUpdateException("User token update failed!");
        return result;
    }

    public string GenerateJwtToken(User user, IEnumerable<string> roles, JwtSettings jwtSettings)
    {
        if(user is null) throw new Exception($"Jwt token generation failed! {nameof(user)} is null!");
        var claims = SetUserClaims(user);
        var roleClaims = SetRoleClaims(roles);
        claims.AddRange(roleClaims);
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddHours(Convert.ToDouble(jwtSettings.ExpirationInHours));
        
        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Issuer,
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken(JwtSettings jwtSettings)
    {
        var refreshToken = new RefreshToken()
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Created = DateTime.Now,
            Expires = DateTime.Now.AddDays(7)
        };
        return refreshToken;
    }
}