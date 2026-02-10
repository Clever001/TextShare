using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Model;
using Auth.Service.Interface;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Service.Impl;

public class TokenService : ITokenService {
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;

    public TokenService(IConfiguration config) {
        _config = config;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
    }

    public string CreateToken(User user) {
        List<Claim> claims = [ // Информация о пользователе
            new(JwtRegisteredClaimNames.GivenName, user.UserName!),
            new(JwtRegisteredClaimNames.Email, user.Email!)
        ];

        // Секретный ключ для создания токена и алгоритм хеширования.
        var signingCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
        // Информация о токене.
        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(claims), // Информация, нужная для определения пользователя.
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = signingCredentials,
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"]
        };
        // Создание объекта, управляющего токенами
        var tokenHandler = new JwtSecurityTokenHandler();
        // Создание токена при помощи информации о токене
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}