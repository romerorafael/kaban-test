using API.Model;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Module.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

public static class BuilderConfig
{
    public static void ConfigureServicesModule(this IServiceCollection services)
    {
        services.AddScoped<IBoardService, BoardService>();
        services.AddScoped<IValidator<Board>, BoardValidator>();

        services.AddScoped<ICardService, CardService>();
        services.AddScoped<IValidator<Card>, CardValidator>();

        services.AddScoped<IColumnService, ColumnService>();
        services.AddScoped<IValidator<Column>, ColumnValidator>();

        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IValidator<Comment>, CommentValidator>();

        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IValidator<Role>,  RoleValidator>();

        services.AddScoped<IUserServices, UserServices>();
        services.AddScoped<IValidator<User>, UserValidator>();

        services.AddScoped<IUserConfigService, UserConfigService>();
        services.AddScoped<IValidator<UserConfig>, UserConfigValidator>();
    }
}
