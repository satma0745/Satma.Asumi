using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Satma.Asumi.Web.Persistence;
using Satma.Asumi.Web.Persistence.Entities;

namespace Satma.Asumi.Web.Services;

public class JwtTokenService
{
    // TODO: Load these parameters from configuration.
    private static readonly TimeSpan accessTokenLifetime = TimeSpan.FromMinutes(5);
    private static readonly TimeSpan refreshTokenLifetime = TimeSpan.FromHours(12);

    private readonly AsumiDbContext dbContext;
    private readonly JwtSigningKeyService jwtSigningKeyService;
    private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler;

    public JwtTokenService(AsumiDbContext dbContext, JwtSigningKeyService jwtSigningKeyService)
    {
        this.dbContext = dbContext;
        this.jwtSigningKeyService = jwtSigningKeyService;
        jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    }

    public async Task<JwtTokenPair> IssueJwtTokenPair(
        Guid bearerId,
        Guid refreshTokenId,
        CancellationToken cancellationToken)
    {
        var accessToken = IssueJwtToken(
            new Dictionary<string, object>
            {
                { JwtRegisteredClaimNames.Sub, bearerId.ToString() }
            },
            accessTokenLifetime
        );

        var refreshToken = IssueJwtToken(
            new Dictionary<string, object>
            {
                { JwtRegisteredClaimNames.Sub, bearerId.ToString() },
                { JwtRegisteredClaimNames.Jti, refreshTokenId.ToString() }
            },
            refreshTokenLifetime
        );
        
        var newUserSession = new UserSession
        {
            Id = Guid.NewGuid(),
            UserId = bearerId,
            RefreshTokenId = refreshTokenId,
            ExpiresAt = DateTime.UtcNow.Add(refreshTokenLifetime)
        };
        dbContext.UserSessions.Add(newUserSession);
        await dbContext.SaveChangesAsync(cancellationToken);

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