using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Northwest.Domain.Dtos;
using Northwest.Persistence.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Northwest.Domain.Authentication;

public class JwtTokenManager : IJwtTokenManager
{
    private readonly JwtConfig _config;
    private readonly JwtSecurityTokenHandler _tokenHandler;
    // IOptions retrievces jwetconfig from apsset
    public JwtTokenManager(IOptions<JwtConfig> jwtConfig)
    {
        _config = jwtConfig.Value;
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    public async Task<string> GenerateToken(UserDto userDto)
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString()),
            new Claim(ClaimTypes.Name, userDto.Name)
        };

        // create claims for each role
        List<Claim> roleClaims = userDto.RoleNames.Select(CreateRoleClaim).ToList();
        claims.AddRange(roleClaims);

        // store security key
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.SecretKey));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        DateTime expires = DateTime.Now.AddDays(Convert.ToDouble(_config.ExpirationDays));

        JwtSecurityToken securityToken = new JwtSecurityToken(
            _config.Issuer,
            _config.Audience,
            claims,
            expires: expires,
            signingCredentials: creds
        );

        string writenToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return writenToken;
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        try
        {
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                //ValidIssuer = _config.Issuer,
                //ValidAudience = _config.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.SecretKey))
            };

            // Validate and parse the token
            ClaimsPrincipal principal = _tokenHandler.ValidateToken(token, validationParameters, out _);

            // Ensure the token has the required claims or perform additional validations

            return principal;
        }
        catch (Exception)
        {
            // Token validation failed
            return null;
        }
    }

    private Claim CreateRoleClaim(RoleName roleName)
    {
        return new Claim(ClaimTypes.Role, roleName.ToString());
    }


}

