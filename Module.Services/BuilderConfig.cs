using API.Model;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Module.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

public static class BuilderConfig
{
    public static void ConfigureServicesModule(this IServiceCollection services)
    {
        services.AddScoped<IUserServices, UserServices>();
        services.AddScoped<IValidator<User>, UserValidator>();
    }
}
