using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog.Events;
using Serilog;
using System.Text;
using Lucky99.Utilities.Cors;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", builder => builder
        //.WithOrigins("https://localhost:44445",
        //"http://localhost:44445",
        //"http://localhost:4200",
        //"http://localhost:44463",
        // "https://localhost:44463",
        //"https://localhost:44463",
        //"https://localhost:4200")
        .AllowAnyOrigin()
        .AllowAnyMethod() // This doesn't explicitly include OPTIONS
        .AllowAnyHeader()
        .WithMethods("OPTIONS", "GET", "POST", "PUT", "DELETE"));


    //.SetPreflightMaxAge(TimeSpan.FromSeconds(3600)));

});


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


// Add services to the container.



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi
        .Models.OpenApiInfo
    { Title = "Luck9 API", Version = "v1" });
});

builder.Services.AddDataBaseServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.ManageDependencyInjection();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("veryverysceret.....")),
        ValidateAudience = false,
        ValidateIssuer = false,
        ClockSkew = TimeSpan.Zero
    };
});

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
app.UseCors("MyPolicy");



app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
