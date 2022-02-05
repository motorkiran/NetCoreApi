using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

public class JWTService : IJWTService
{
    private readonly AppSettings _appSettings;

    public JWTService(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
    }

    public string GetToken(LoginDto loginDto)
    {
        Claim[] claims;

        claims = new[]
        {
                    new Claim(JwtRegisteredClaimNames.Email, loginDto.Email),
                    new Claim(JwtRegisteredClaimNames.CHash, loginDto.Password),
            };

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.JwtKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}