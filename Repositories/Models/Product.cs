namespace MinhaAPI.Repositories.Models;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsEnabled { get; set; }

    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
}