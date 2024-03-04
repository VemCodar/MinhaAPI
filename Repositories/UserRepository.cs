using MinhaAPI.Models;

namespace MinhaAPI.Repositories;

public class UserRepository
{
    public User? GetByEmailAndPassword(string email, string password)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            Password = string.Empty,
            Role = "admin"
        };
    }

    public User? GetByEmail(string email)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            Password = string.Empty,
            Role = "admin"
        };
    }
}