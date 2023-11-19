using Lucky9.Application.Common.Interfaces;
using Lucky9.Infrastructure.Data;
using Lucky9.Infrastructure.Identity;
using Lucky9.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;
public static class ConfigureDataServices
{
    public static IServiceCollection AddDataBaseServices(this IServiceCollection services, IConfiguration configuration)
    {

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("Lucky9Db"));
        }
        else
        {    //_Step#1 db connections
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"),
                    //options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),

                    builder => builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
        }
        services.AddScoped<IDataContext>(provider => provider.GetRequiredService<AppDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();


        return services;
    }
}
