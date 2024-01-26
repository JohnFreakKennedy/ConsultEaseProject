namespace ConsultEaseBLL.DTOs.Authentication;

public class UserSignInDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set;} = string.Empty;
}