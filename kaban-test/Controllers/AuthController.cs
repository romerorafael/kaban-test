using API.Model;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Module.Auth;
using OneOf.Types;

namespace kaban_test.Controllers;

[ApiController]
[Route("auth")]
public class AuthController
{
    [HttpPost]
    public IResult Login(
        [FromBody] UserDTO userDTO,
        [FromServices] IAuthService authService,
        [FromServices] IMapper _mapper
    )
    {
        User user = _mapper.Map<User>(userDTO); 
        var resultRequest = authService.AuthenticateUser(user);

        return resultRequest.Match(
            tokenResponse => Results.Ok(tokenResponse),
            error => { return Results.BadRequest(); }
        );
    }
}
