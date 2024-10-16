using API.Model;
using API.OneOfErrors;
using OneOf;
using System.Security.Claims;

namespace Module.Auth;

public interface IAuthService
{
    OneOf<TokenResponse, AppError> AuthenticateUser(User user);
    Task<OneOf<bool, AppError>> RestartPassword(RestartPassword restartPassword);
    bool ValidateToken(string token, string privateKey, out ClaimsPrincipal? principal);
    string CodePassword(string password, Guid guid);
}
