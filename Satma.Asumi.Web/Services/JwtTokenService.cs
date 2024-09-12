using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Satma.Asumi.Web.Services;

public class JwtTokenService
{
    private readonly JwtSigningKeyService jwtSigningKeyService;
    private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler;

    public JwtTokenService(JwtSigningKeyService jwtSigningKeyService)
    {
        this.jwtSigningKeyService = jwtSigningKeyService;
        jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    }

    public JwtTokenPair IssueJwtTokenPair(Guid bearerId, Guid refreshTokenId)
    {
        var accessToken = IssueJwtToken(
            new Dictionary<string, object>
            {
                { JwtRegisteredClaimNames.Sub, bearerId.ToString() }
            },
            TimeSpan.FromMinutes(5)
        );

        var refreshToken = IssueJwtToken(
            new Dictionary<string, object>
            {
                { JwtRegisteredClaimNames.Sub, bearerId.ToString() },
                { JwtRegisteredClaimNames.Jti, refreshTokenId.ToString() }
            },
            TimeSpan.FromHours(5)
        );

        return new JwtTokenPair
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
    
    private string IssueJwtToken(IDictionary<string, object> claims, TimeSpan lifetime)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = "Satma.Asumi",
            Claims = claims,
            Expires = DateTime.UtcNow.Add(lifetime),
            Issuer = "Satma.Asumi",
            SigningCredentials = new SigningCredentials(
                key: new RsaSecurityKey(jwtSigningKeyService.SigningKey),
                algorithm: SecurityAlgorithms.RsaSha512Signature
            )
        };
        
        var token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
        
        return jwtSecurityTokenHandler.WriteToken(token);
    }
}

public class JwtTokenPair
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}