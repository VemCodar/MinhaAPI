namespace MinhaAPI.Filters;

public class ValidationFilter<T> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context, 
        EndpointFilterDelegate next)
    {
        var model = context.Arguments.FirstOrDefault(x => x is T);
        if (model is null) return await next(context);
        
        var isValid = MiniValidation.MiniValidator.TryValidate(model, out var errors);
        if (!isValid)
            return Results.BadRequest(errors);
        
        return await next(context);
    }
}