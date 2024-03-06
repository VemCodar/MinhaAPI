using Microsoft.EntityFrameworkCore;

namespace MinhaAPI.Repositories;

public class ProductsDbContext : DbContext
{
    public DbSet<Models.Category> Categories => Set<Models.Category>();
    public DbSet<Models.Product> Products => Set<Models.Product>();

    public ProductsDbContext(DbContextOptions<ProductsDbContext> options) 
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        MapProducts(modelBuilder);
        MapCategories(modelBuilder);
    }

    private static void MapProducts(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Models.Product>()
            .HasQueryFilter(p => p.IsEnabled)
            .ToTable("tb_products");

        modelBuilder.Entity<Models.Product>()
            .HasKey(p => p.Id)
            ;
        
        modelBuilder.Entity<Models.Product>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id")
            ;
        
        modelBuilder.Entity<Models.Product>()
            .Property(p => p.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasColumnType("nvarchar(50)") 
            // Importante usar o tipo correto caso seja necessário alguma consulta com a coluna
            ;
        
        modelBuilder.Entity<Models.Product>()
            .Property(p => p.CategoryId)
            .HasColumnName("category_id")
            ;
        
        modelBuilder.Entity<Models.Product>()
            .Property(p => p.IsEnabled)
            .HasColumnName("is_enabled")
            ;

        modelBuilder.Entity<Models.Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products);
    }
    
    private static void MapCategories(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Models.Category>()
            .HasQueryFilter(p => p.IsEnabled)
            .ToTable("tb_categories");

        modelBuilder.Entity<Models.Category>()
            .HasKey(p => p.Id)
            ;
        
        modelBuilder.Entity<Models.Category>()
            .Property(p => p.Id)
            .HasColumnName("id")
            ;
        
        modelBuilder.Entity<Models.Category>()
            .Property(p => p.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(50)
            ;
        
        modelBuilder.Entity<Models.Category>()
            .Property(p => p.IsEnabled)
            .HasColumnName("is_enabled")
            ;

        modelBuilder.Entity<Models.Category>()
            .HasMany(p => p.Products)
            .WithOne(c => c.Category);
    }
}