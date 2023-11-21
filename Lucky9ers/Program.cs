using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog.Events;
using Serilog;
using System.Text;
using Lucky99.Utilities.Cors;
using api.Startup;

var builder = WebApplication.CreateBuilder(args);


#region  CORS ----------------------------
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder
        .SetIsOriginAllowedToAllowWildcardSubdomains()
        //.WithOrigins("https://localhost:44418/")
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
        //.AllowCredentials()
        .WithMethods("OPTIONS", "GET", "POST", "PUT", "DELETE")
        .Build());
});

////builder.Services.AddCors(options =>
////{
////    options.AddPolicy("MyPolicy", builder => builder
////        //.WithOrigins("https://localhost:44445",
////        //"http://localhost:44445",
////        //"http://localhost:4200",
////        //"http://localhost:44463",
////        // "https://localhost:44463",
////        //"https://localhost:44463",
////        //"https://localhost:4200")
////        .AllowAnyOrigin()
////        .AllowAnyMethod() // This doesn't explicitly include OPTIONS
////        .AllowAnyHeader()
////        .WithMethods("OPTIONS", "GET", "POST", "PUT", "DELETE"));
////});
#endregion

//_step #1 enable logging
#region ----------Configure Logging-----------

var appDirectory = System.AppContext.BaseDirectory;
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    //.WriteTo.AzureAnalytics(workspaceId, primaryKey, logName)
    .WriteTo.Console()

    // Add a log file that will be replaced by a new log file each day
    .WriteTo.File(Path.Combine(appDirectory, "all-daily-.logs"),
        shared: true,
        //restrictedToMinimumLevel: LogEventLevel.Error,
        restrictedToMinimumLevel: LogEventLevel.Information,

        rollingInterval: RollingInterval.Day)

    .CreateLogger();

builder.Host.UseSerilog();

#endregion

//_step #2 setp configruations for environments
builder.Host.ConfigureAppConfigurationFromCustomFile();
AppGlobal.Environment = builder.Environment;

//_step #3 Cofigure services to add
builder.Services.ConfigureServices();
builder.Services.AddDataBaseServices(builder.Configuration);

//End configurations--------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseMiddleware<CustomCorsMiddleware>();

app.UseStaticFiles();
app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lucky9 Api V1");
});


app.UseHttpsRedirection();
app.UseCors();



app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
