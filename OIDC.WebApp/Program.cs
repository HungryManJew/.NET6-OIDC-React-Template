using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using OIDC.WebApp.Clients;
using OIDC.WebApp.Services;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Initializing Configuration
_configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
        .AddEnvironmentVariables()
        .Build();

// Configure Serilog
builder.Host.UseSerilog((ctx, lc) =>
    lc.ReadFrom.Configuration(_configuration)
);

// Add AuthorizationService
builder.Services.AddAuthorization();

// Configure the Cookie Policy
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

// Adding the Authentication Service
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = OpenIdConnectDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie()
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.Authority = configuration["OidcConfig:Authority"]; // From User Secrets
    options.ClientId = configuration["OidcConfig:ClientId"]; // From User Secrets
    options.ClientSecret = configuration["OidcConfig:ClientSecret"]; // From User Secrets

    options.MetadataAddress = configuration["OidcConfig:MetadataAddress"]; // From User Secrets

    options.UsePkce = true; // If optional
    options.ResponseType = "code";
    options.RequireHttpsMetadata = false;
});


// Add services to the container.
builder.Services.AddControllersWithViews();

// Adding Swagger
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adding AutoMapper as a service for the project
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Controller Services
#region ControllerServices

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<WeatherForecastService>();

#endregion

// Services HTTP Clients
#region ServicesClients

builder.Services.AddHttpClient<WeatherForecastClient>(client =>
{
    var baseAddress = new Uri(_configuration["urls:weatherforecast"]); // From appsettings.json

    client.BaseAddress = baseAddress;
});

#endregion

var app = builder.Build();

// Enable Swagger in development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// AuthN & AuthZ Related configuration
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");;

app.Run();

partial class Program
{
    private static IConfiguration? _configuration { get; set; }
    public static IConfiguration? configuration { get { return _configuration; } }
}