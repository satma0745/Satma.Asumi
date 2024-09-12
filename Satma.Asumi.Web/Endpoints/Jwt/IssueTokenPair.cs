using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Satma.Asumi.Web.Persistence;
using Satma.Asumi.Web.Services;

namespace Satma.Asumi.Web.Endpoints.Jwt;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class IssueTokenPairController(
    AsumiDbContext dbContext,
    JwtTokenService jwtTokenService,
    PasswordService passwordService) : ControllerBase
{
    [HttpPost("/api/jwt/issue-token-pair")]
    [ProducesResponseType<JwtTokenPairDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> IssueTokenPair(
        [FromBody] [Required] UserCredentialsDto userCredentialsDto,
        CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .Where(user => user.Email == userCredentialsDto.Email)
            .SingleOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            ModelState.AddModelError("$", "Invalid credentials provided.");
            return ValidationProblem();
        }
        if (!passwordService.DoPasswordsMatch(userCredentialsDto.Password, user.Password))
        {
            ModelState.AddModelError("$", "Invalid credentials provided.");
            return ValidationProblem();
        }

        // TODO: save Refresh Token into a DB.
        var refreshTokenId = Guid.NewGuid();

        var jwtTokenPair = jwtTokenService.IssueJwtTokenPair(user.Id, refreshTokenId);
        var jwtTokenPairDto = new JwtTokenPairDto
        {
            AccessToken = jwtTokenPair.AccessToken,
            RefreshToken = jwtTokenPair.RefreshToken
        };
        return Ok(jwtTokenPairDto);
    }

    public class UserCredentialsDto
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
    }

    public class JwtTokenPairDto
    {
        public required string AccessToken { get; init; }
        public required string RefreshToken { get; init; }
    }
}