using System.ComponentModel.DataAnnotations;

namespace MinhaAPI.Models;

public class Login
{
    [Required, EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}

public class RefreshedToken
{
    [Required]
    public string Token { get; set; }
}