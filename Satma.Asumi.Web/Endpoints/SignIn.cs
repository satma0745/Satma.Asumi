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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SignIn(
        [FromBody] [Required] UserCredentialsDto userCredentialsDto,
        CancellationToken cancellationToken)
    {
        var userExists = await dbContext.Users
            .Where(user => user.Email == userCredentialsDto.Email)
            .AnyAsync(cancellationToken);

        return userExists ? Ok() : NotFound();
    }

    public class UserCredentialsDto
    {
        public required string Email { get; init; }
    }
}