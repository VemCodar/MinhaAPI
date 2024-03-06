using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MinhaAPI.Filters;
using MinhaAPI.Models;
using MinhaAPI.Repositories;
using MinhaAPI.Repositories.Models;
using Product = MinhaAPI.Repositories.Models.Product;

namespace MinhaAPI.Endpoints;

public static class Products
{
    public static IEndpointRouteBuilder AddProductsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (
                [FromBody] Product product,
                [FromServices]IProductsWriteRepository repository,
                CancellationToken cancellationToken) =>
            {
                var newProduct = await repository.AddAsync(product, cancellationToken);
                return Results.Created("/products",product.Id.ToString());
            })
            //.RequireAuthorization("admin")
            .WithName("products_post")
            .WithOpenApi();
        
        app.MapGet("/products", async (
                [FromServices]IProductsReadRepository repository,
                CancellationToken cancellationToken) =>
            {
                var products = await repository.GetAllAsync(cancellationToken);
                return Results.Ok(products);
            })
            //.RequireAuthorization("admin")
            .WithName("products")
            .WithOpenApi();
       
        app.MapGet("/products/{id:guid}", async (
                Guid id,
                [FromServices]IProductsReadRepository repository,
                CancellationToken cancellationToken) =>
            {
                var product = await repository.GetByIdAsync(id, cancellationToken);
                return product is null 
                    ? Results.NotFound() 
                    : Results.Ok(product);
            })
            .CacheOutput()
            //.RequireAuthorization("admin")
            .WithName("products_by_id")
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