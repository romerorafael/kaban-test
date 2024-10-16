using API.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Module.Auth;

public static class BuildConfig
{
    public static void ConfigureAuthenticatedModule(this IServiceCollection service, IConfiguration configuration)
    {
        var tokenParams = configuration.GetSection("Auth").GetSection("DataSchema").Value;

        service.AddSingleton<TokenParams>(sp => new TokenParams()
        {
            PrivateKey = configuration.GetSection("Auth").GetSection("PrivateKey").Value ?? "",
            ExpireMinutes = int.Parse(configuration.GetSection("Auth").GetSection("ExpireMinutes").Value ?? "0")
        }
        );

        AuthenticationBuilder authenticationBuilder = service.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        });

        authenticationBuilder.ConfigureJwtBearer(configuration);

        service.AddScoped<IAuthService, AuthService>();
    }

    private static void ConfigureJwtBearer(this AuthenticationBuilder authenticationBuilder, IConfiguration configuration)
    {
        byte[] key = Encoding.ASCII.GetBytes(configuration.GetSection("Auth").GetSection("PrivateKey").Value ?? "");
        authenticationBuilder.AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
            };
        });
    }
}
