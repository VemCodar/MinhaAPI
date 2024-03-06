using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MinhaAPI.Repositories.Models;

namespace MinhaAPI.Repositories;

public interface IProductsReadRepository
{
    Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}

public interface IProductsWriteRepository
{
    Task<Product> AddAsync(Product product, CancellationToken cancellationToken);
}

internal class ProductsRepository(ProductsDbContext dbContext, IDistributedCache cache) 
    : IProductsReadRepository, IProductsWriteRepository
{
    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken)
    {
        const string cacheKey = "products";
        
        var json = await cache.GetStringAsync(cacheKey, cancellationToken);
        if (!string.IsNullOrEmpty(json)) 
            return JsonSerializer.Deserialize<IEnumerable<Product>>(json) ?? [];
        
        var products = await GetProductsQuery().ToListAsync(cancellationToken);
        json = JsonSerializer.Serialize(products);
        await cache.SetStringAsync(cacheKey, json, new DistributedCacheEntryOptions()
        {
            SlidingExpiration = TimeSpan.FromSeconds(10),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
        }, cancellationToken);
        return products;
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = await GetProductsQuery().FirstOrDefaultAsync(cancellationToken);
        return product;
    }

    public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken)
    {
        await dbContext.Products.AddAsync(product, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return product;
    }

    private IQueryable<Product> GetProductsQuery()
        => dbContext
            .Products
            .AsSplitQuery()
            //.Include(p => p.Category)
            //.AsNoTracking() // Evita mapeamento de propriedades internas sem necessidade
            .AsNoTrackingWithIdentityResolution();
}