//using api.Utilities.JwtToken;
using api.Startup;
using api.Utilities.JwtToken;
using Lucky9.Application.Common.Models;
using Lucky9.Application.Interfaces;
using Lucky9.Application.IServices;
using Lucky9.Application.Utilities.Swagger;
using Lucky9.Infrastructure.Data;
using Lucky9.Infrastructure.Persistence;
using Lucky9.Infrastructure.Services;
using Lucky99.Utilities.Cors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class StartUp
{
  
    public static void ConfigureServices(this IServiceCollection services)
    {

        var appSettings = AppGlobal.Settings;
       var _config = AppGlobal.Configuration;

        // Add services to the container...

        //basic infra
        services.AddApplicationServices();

        //database specific
        services.AddDataBaseServices(_config);

        //DI 
        StartUp.ManageDependencyInjection(services);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();

        //_step#1 add security using jwt token 
        #region Authentication/Authorization

        {
            var authSetting = appSettings.JWT;

            services
                .AddAuthentication(opts =>
                {
                    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        // standard configuration
                        ValidIssuer = authSetting.Issuer,
                        ValidAudience = authSetting.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(authSetting.Secret)),

                        // security switches
                        //RequireExpirationTime = true,
                        //ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = true
                    };

                    cfg.SaveToken = true;
                    cfg.ClaimsIssuer = cfg.TokenValidationParameters.ValidIssuer;

                });

            // api user claim policy
            services.AddAuthorization(options => { });
        }

        #endregion




        #region Swagger       

        services.AddControllers();


        services.AddSwaggerGen(swag =>
        {

            swag.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            swag.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });


            swag.OperationFilter<ReApplyOptionalRouteParameterOperationFilter>();


        });

        #endregion

        // cors
       

        //make http context available
        services.AddHttpContextAccessor();

        //services.Configure<AppSettings>(AppGlobal.Configuration);


    }

    public static void ManageDependencyInjection( this IServiceCollection services)
    {
        var _appSettings = AppGlobal.Configuration
              .Get<AppSettings>();       
        services.AddSingleton<AppSettings>(_appSettings);


        services.AddTransient<CustomCorsMiddleware>();
        services.AddScoped<DataSeeder>();

        services.AddScoped<IPlayerRepository, EfCorePlayerRepository>();
        services.AddScoped<IBetRepository, EfCoreBetRepository>();
        services.AddScoped<IGameRepository, EfCoreGameRepository>();
        services.AddScoped<EfCorePlayerRepository>();
        services.AddScoped<EfCoreBetRepository>();
        services.AddScoped<EfCoreGameRepository>();

        services.AddScoped<IGameService, GameService>();



    }
}
