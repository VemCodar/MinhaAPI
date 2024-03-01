using System.Text;

namespace MinhaAPI.Models;

public class AuthConfig(IConfiguration configuration)
{
    public byte[] CreateSecretKeyBytes()
    {
        var secretKey = configuration["SecretKey"] ?? throw new ArgumentNullException();
        return Encoding.ASCII.GetBytes(secretKey);
    }
}