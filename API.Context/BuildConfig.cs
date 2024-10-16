using API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Context;

public static class BuildConfig
{
    public static void ConfigureContextModule(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
            x => x.MigrationsHistoryTable("_EfMigrations", configuration.GetSection("Schema").GetSection("DataSchema").Value)
            )
        );       

        service.AddScoped<IAppDbContext, AppDbContext>();
    }
}
