using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MinhaAPI.Setttings;

[ExcludeFromCodeCoverage]
public static class SwaggerSettings
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new() { Title = "My API", Version = "v1" });
            var xml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xml);
            opt.IncludeXmlComments(xmlPath);
            opt.TagActionsBy(d =>
            {
                return new List<string>() { d.ActionDescriptor.DisplayName! };
            });
        });
        return services;
    }

    public static WebApplication UseSwaggerAPI(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    }
}