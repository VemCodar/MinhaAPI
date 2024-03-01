using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using MinhaAPI.Models;

namespace MinhaAPI.Services;

public class TokenService(AuthConfig authConfig)
{
    public string CreateToken(User user)
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
            issuer: "MinhaAPI",
            audience: "MinhaAPI",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );
        return tokenHandler.WriteToken(token);
    }
}