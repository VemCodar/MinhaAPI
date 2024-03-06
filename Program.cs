using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MinhaAPI.Endpoints;
using MinhaAPI.Middlewares;
using MinhaAPI.Models;
using MinhaAPI.Repositories;
using MinhaAPI.Services;
using MinhaAPI.Setttings;

var builder = WebApplication.CreateBuilder(args);
builder.AddLogSettings();

builder.Services.AddAuthenticationJWT(builder.Configuration);
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ExceptionGlobalHandler>();
builder.Services.AddScoped<AuthConfig>(_ => new AuthConfig(builder.Configuration));
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddRepositories(builder.Configuration);

//builder.Services.AddDistributedMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
   
builder.Services.AddSwagger();
var app = builder.Build();
app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();
app.AddProductsEndpoints()
   .AddAuthEndpoints();
app.UseSwaggerAPI();
app.Run();
