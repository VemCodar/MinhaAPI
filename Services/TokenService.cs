using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using MinhaAPI.Models;

namespace MinhaAPI.Services;

public class TokenService(AuthConfig authConfig)
{
    public string CreateToken(User user, int expiresInMinutes = 30)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var securityKey = authConfig.CreateSecretKeyBytes();
        var key = new SymmetricSecurityKey(securityKey);
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("aluno", "vemcodar"),
        };
        
        var token = new JwtSecurityToken(
            issuer: authConfig.Issuer,
            audience: authConfig.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(expiresInMinutes),
            signingCredentials: credentials
        );
        return tokenHandler.WriteToken(token);
    }
    
    public string ExtractEmailFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        if(securityToken is null) return string.Empty;
        var claim = securityToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
        return claim?.Value ?? string.Empty;
    }
}