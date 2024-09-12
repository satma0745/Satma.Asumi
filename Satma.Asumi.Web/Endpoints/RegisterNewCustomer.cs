using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Satma.Asumi.Web.Persistence;
using Satma.Asumi.Web.Persistence.Entities;
using Satma.Asumi.Web.Services;

namespace Satma.Asumi.Web.Endpoints;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class RegisterNewCustomer(AsumiDbContext dbContext, PasswordService passwordService) : ControllerBase
{
    [HttpPost("/api/register-new-customer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignIn(
        [FromBody] [Required] UserRegistrationDto userRegistrationDto,
        CancellationToken cancellationToken)
    {
        var emailIsAlreadyInUse = await dbContext.Users
            .Where(user => EF.Functions.ILike(user.Email, userRegistrationDto.Email))
            .AnyAsync(cancellationToken);

        if (emailIsAlreadyInUse)
        {
            ModelState.AddModelError(nameof(userRegistrationDto.Email), "Provided E-Mail is already in use.");
            return ValidationProblem();
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            DisplayName = userRegistrationDto.DisplayName,
            PhoneNumber = userRegistrationDto.PhoneNumber,
            Email = userRegistrationDto.Email,
            Password = passwordService.HashPassword(userRegistrationDto.Password),
            Role = UserRole.Customer
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Ok();
    }

    public class UserRegistrationDto
    {
        public required string DisplayName { get; init; }
        public required string PhoneNumber { get; init; }
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}