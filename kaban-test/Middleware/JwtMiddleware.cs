using API.Model;
using Microsoft.AspNetCore.Mvc;
using Module.Auth;
using System.Security.Claims;

namespace kaban_test;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TokenParams _client;

    public JwtMiddleware(RequestDelegate next, TokenParams options)
    {
        _next = next;
        _client = options;
    }

    public async Task Invoke(HttpContext context, [FromServices] IAuthService authService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            ClaimsPrincipal claimsPrincipal;
            bool isValid = authService.ValidateToken(token, _client.PrivateKey!, out claimsPrincipal!);

            if (isValid)
            {
                context.User = claimsPrincipal!;
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = 401; // Unauthorized
            }

        }
        else
        {
            context.Response.StatusCode = 401; // Unauthorized
        }
    }

}
