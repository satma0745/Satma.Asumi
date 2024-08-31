using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Satma.Asumi.Web.Persistence;

namespace Satma.Asumi.Web.Endpoints;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class SignInController(AsumiDbContext dbContext) : ControllerBase
{
    [HttpPost("/api/sign-in")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignIn(
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
        if (!BcryptNet.EnhancedVerify(userCredentialsDto.Password, user.Password))
        {
            ModelState.AddModelError("$", "Invalid credentials provided.");
            return ValidationProblem();
        }

        return Ok();
    }

    public class UserCredentialsDto
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}