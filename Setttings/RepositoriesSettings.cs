using Microsoft.EntityFrameworkCore;
using MinhaAPI.Repositories;

namespace MinhaAPI.Setttings;

public static class RepositoriesSettings
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<ProductsDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if(string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("Connection string not found");
            
            options.UseSqlServer(connectionString, o =>
            {
                o.EnableRetryOnFailure(3);
            });
        
#if DEBUG
            options.LogTo(Console.WriteLine);
#endif
        });
        
        services.AddScoped<IProductsReadRepository, ProductsRepository>();
        services.AddScoped<IProductsWriteRepository, ProductsRepository>();
        return services;
    }
}