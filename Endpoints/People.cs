using System.Diagnostics.CodeAnalysis;

namespace MinhaAPI.Endpoints;

[ExcludeFromCodeCoverage]
public static class People
{
    public static IEndpointRouteBuilder AddPeopleEndpoints(this IEndpointRouteBuilder app)
    {
        // app.MapGet("/people", () => { })
        //     .AddEndpointFilter<ValidationFilter<PersonDTO>>()
        //     .WithName("people")
        //     .WithOpenApi();

        return app;
    }
}