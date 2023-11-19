//using api.Utilities.JwtToken;
using api.Utilities.JwtToken;
using Lucky9.Application.Interfaces;
using Lucky9.Application.IServices;
using Lucky9.Infrastructure.Data;
using Lucky9.Infrastructure.Persistence;
using Lucky9.Infrastructure.Services;
using Lucky99.Utilities.Cors;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class StartUp
{
    public static void ManageDependencyInjection( this IServiceCollection services)
    {
        // _step #5 all specific dependecy for carePatron application should start here...
        // ioc
        //services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase(databaseName: "Test"));

        services.AddTransient<CustomCorsMiddleware>();
        services.AddScoped<DataSeeder>();

        services.AddScoped<IPlayerRepository, EfCorePlayerRepository>();

        services.AddScoped<IBetRepository, EfCoreBetRepository>();
        services.AddScoped<IGameRepository, EfCoreGameRepository>();


        services.AddScoped<EfCorePlayerRepository>();

        services.AddScoped<EfCoreBetRepository>();
        services.AddScoped<EfCoreGameRepository>();



        services.AddScoped<IGameService, GameService>();

        services.AddScoped<ISecurityService, IdentityHelper>();


    }
}
