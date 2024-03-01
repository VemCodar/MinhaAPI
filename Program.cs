using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

builder.Services.AddScoped<AuthConfig>(_ => new AuthConfig(builder.Configuration));
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<TokenService>();

builder.Services.AddSwagger();
var app = builder.Build();

//app.AddExceptionHandler<ExceptionGlobalHandler>();
app.UseAuthentication();
app.UseAuthorization();
app.AddPeopleEndpoints()
   .AddProductsEndpoints()
   .AddAuthEndpoints();
app.UseSwaggerAPI();
app.Run();
