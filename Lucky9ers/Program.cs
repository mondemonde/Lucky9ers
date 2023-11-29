using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog.Events;
using Serilog;
using System.Text;
using Lucky99.Utilities.Cors;
using api.Startup;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Security.Authentication;
using System.Net;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

    #region HTTPS for kestrel-------------------------------------

//    // Configure Kestrel with HTTPS and specify the certificate
//    var dirExe = AppDomain.CurrentDomain.BaseDirectory;
//    var certificateFilePath = Path.Combine(dirExe, "ssl", "server.crt");
//    var privateKeyFilePath = Path.Combine(dirExe, "ssl", "server.key");

//if (!File.Exists(certificateFilePath) || !File.Exists(privateKeyFilePath))
//{
//    throw new FileNotFoundException("Certificate or private key file not found");
//}

//var certificate = new X509Certificate2(certificateFilePath, File.ReadAllText(privateKeyFilePath));

//builder.WebHost.UseKestrel(options =>
//{
//    options.ConfigureHttpsDefaults(httpsOptions =>
//    {
//        httpsOptions.ServerCertificate = certificate;
//    });
//});



#endregion---------------------------------------------------



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
