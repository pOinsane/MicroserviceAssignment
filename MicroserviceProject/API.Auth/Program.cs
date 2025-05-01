using APP.Auth.Features.Login;
using APP.Auth.Services;
using APP.Users.Context;
using CORE.APP.Common;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire Service Defaults (if you're using Aspire)
builder.AddServiceDefaults();

// Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins("https://localhost:7164") // your frontend origin
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // needed for cookies
    });
});

// Database for Users
builder.Services.AddDbContext<UsersDb>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Register MediatR for Auth Handlers
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(UserLoginHandler).Assembly);
});

// Register IHttpContextAccessor for cookie handling
builder.Services.AddHttpContextAccessor();

// Inject AuthService
builder.Services.AddScoped<AuthService>();

// Configure cookie-based JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = AppSettings.Issuer,
            ValidAudience = AppSettings.Audience,
            IssuerSigningKey = AppSettings.SigningKey,
            RoleClaimType = ClaimTypes.Role,           
            ClockSkew = TimeSpan.FromMinutes(5)           
        };


        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.HttpContext.Request.Cookies["jwt"];
                return Task.CompletedTask;
            }
        };
    });

// Optional: suppress automatic model validation
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication(); // JWT
app.UseAuthorization();
app.MapControllers();
app.Run();
