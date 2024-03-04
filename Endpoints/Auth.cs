using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MinhaAPI.Filters;
using MinhaAPI.Models;
using MinhaAPI.Repositories;
using MinhaAPI.Services;

namespace MinhaAPI.Endpoints;

public static class Auth
{
    public static IEndpointRouteBuilder AddAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/auth", (
                [FromBody] Login login,
                [FromServices]UserRepository userRepository,
                [FromServices]TokenService tokenService) =>
            {
                var user = userRepository.GetByEmailAndPassword(login.Email, login.Password);
                if (user is null) return Results.Unauthorized();

                var expiresInMinutes = 30;
                var token = tokenService.CreateToken(user, expiresInMinutes);
                var refreshedToken = tokenService.CreateToken(user, expiresInMinutes + 5);
                var tokenResult = new TokenResult(token, refreshedToken, user.Email);
                return Results.Ok(tokenResult);
            })
            .AddEndpointFilter<ValidationFilter<Login>>()
            .WithName("auth")
            .WithOpenApi();
        
        app.MapPost("/auth/refresh", (
                [FromBody] RefreshedToken refreshedToken,
                [FromServices]UserRepository userRepository,
                [FromServices]TokenService tokenService) =>
            {
                var email = tokenService.ExtractEmailFromToken(refreshedToken.Token);
                var user = userRepository.GetByEmail(email);
                if (user is null) return Results.Unauthorized();

                var token = tokenService.CreateToken(user, 5);
                var tokenResult = new TokenResult(token, string.Empty, user.Email);
                return Results.Ok(tokenResult);
            })
            .AddEndpointFilter<ValidationFilter<Login>>()
            .WithName("auth-refresh")
            .WithOpenApi();

        return app;
    }
}