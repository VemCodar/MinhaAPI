using System.Security.Claims;
using MinhaAPI.Filters;
using MinhaAPI.Models;

namespace MinhaAPI.Endpoints;

public static class Products
{
    public static IEndpointRouteBuilder AddProductsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/products", (ClaimsPrincipal user) =>
            {
                return Results.Ok(user.Identity);
            })
            .RequireAuthorization("admin")
            .WithName("products")
            .WithOpenApi();
        
        app.MapGet("/products/exceptions", (ClaimsPrincipal user) =>
            {
                throw new Exception();
            })
            .WithName("productsexceptions")
            .WithOpenApi();        

        return app;
    }
}