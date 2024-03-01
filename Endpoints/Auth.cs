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
                
                var token = tokenService.CreateToken(user);
                var tokenResult = new TokenResult(token, user.Email);
                return Results.Ok(tokenResult);
            })
            .AddEndpointFilter<ValidationFilter<Login>>()
            .WithName("login")
            .WithOpenApi();

        return app;
    }
}