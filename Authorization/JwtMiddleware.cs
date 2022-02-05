using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppSettings _appSettings;
    private readonly ILogService _logService;

    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings, ILogService logService)
    {
        _next = next;
        _appSettings = appSettings.Value;
        _logService = logService;
    }

    public async Task Invoke(HttpContext context, IUserService userService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            AttachUserToContext(context, userService, token);

        await _next(context);
    }

    private void AttachUserToContext(HttpContext context, IUserService userService, string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.JwtKey);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var email = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value.ToString();
            var password = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.CHash).Value.ToString();

            var user = userService.GetUserByEmailAndPassword(email, password).Result as User;
            context.Items["User"] = userService.GetUserByEmailAndPassword(email, password).Result;
        }
        catch (Exception ex)
        {
            _logService.Error(ex.Message);
        }
    }
}