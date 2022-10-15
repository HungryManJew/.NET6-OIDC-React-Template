using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OIDC.ClassLib.Helpers;
using OIDC.ClassLib.Middlewares;
using Serilog;
using System.Reflection;
using System.Security.Cryptography;

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

// Add Cors as a Service
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "WebAppHosts",
        policy => {
            policy.WithOrigins(_configuration.GetSection("AllowedCORSHosts").Get<string[]>()); // From appsettings.json
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowCredentials();
        });
});

// Configuring RSA Service
builder.Services.AddSingleton<RsaSecurityKey>(provider => {
    RSA rsa = RSA.Create();
    rsa.ImportRSAPublicKey(
        source: Convert.FromBase64String(configuration["JwtConfig:PublicKey"]),
        bytesRead: out int _
    );
    return new RsaSecurityKey(rsa);
});

// Configuring Authenticvation Service
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => {
    SecurityKey rsa = builder.Services.BuildServiceProvider().GetRequiredService<RsaSecurityKey>();

    options.IncludeErrorDetails = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidAudience = configuration["JwtConfig:Audience"], // From User Secrets
        ValidIssuer = configuration["JwtConfig:Authority"], // From User Secrets
        IssuerSigningKey = rsa,
        RequireSignedTokens = true,
        RequireExpirationTime = true,
        ValidateLifetime = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true,
    };

    options.Authority = configuration["JwtConfig:Authority"]; // From User Secrets
    options.Audience = configuration["JwtConfig:Audience"]; // From User Secrets
});

builder.Services.AddAuthorization();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Adding RequestResponseLoggingMiddleware ft. Serilog
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseSerilogRequestLogging(opts => {
    opts.EnrichDiagnosticContext = LogHelper.EnrichFromRequest;
    opts.MessageTemplate = "Handled {Protocol} {RequestGuid} {RequestPath} - {RequestBody} with Status Code {StatusCode} in {Elapsed}";
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("WebAppHosts");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

partial class Program
{
    private static IConfiguration? _configuration { get; set; }
    public static IConfiguration? configuration { get { return _configuration; } }
}