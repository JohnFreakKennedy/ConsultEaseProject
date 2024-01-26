using ConsultEaseBLL.DTOs.Authentication;
using ConsultEaseDAL.Entities.Auth;

namespace ConsultEaseBLL.Interfaces.Authentication;

public interface IAuthService
{
    Task<string> SignUpAsync(UserSignUpDto userDto);
    Task<User> SignInAsync(UserSignInDto userDto);
    Task SignOutAsync();
}