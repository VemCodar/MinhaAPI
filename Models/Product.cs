using System.ComponentModel.DataAnnotations;

namespace MinhaAPI.Models;

/// <summary>
/// DTO de Produto
/// </summary>
public class Product
{
    /// <summary>
    /// Nome do produto
    /// </summary>
    [Required(ErrorMessage = "Nome é obrigatório")]
    [MaxLength(30)]
    [MinLength(5)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Descrição do Produto
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Preço do Produto
    /// </summary>
    [Range(1, 999)]
    public decimal Price { get; set; }
}