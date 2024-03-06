namespace MinhaAPI.Repositories.Models;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsEnabled { get; set; }
    
    public ICollection<Product> Products { get; set; } = new List<Product>();
}