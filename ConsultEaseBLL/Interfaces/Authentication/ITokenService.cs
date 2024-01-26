using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using ConsultEaseDAL.Entities.Auth;
using ConsultEaseBLL.DTOs.Authentication;
using ConsultEaseBLL.Helpers;

namespace ConsultEaseBLL.Interfaces.Authentication;

public interface ITokenService
{
    Task<IdentityResult> UpdateUserRefreshTokenAsync(User user, RefreshToken token);
    string GenerateJwtToken(User user, IEnumerable<string> roles, JwtSettings jwtSettings);
    RefreshToken GenerateRefreshToken(JwtSettings jwtSettings);
}