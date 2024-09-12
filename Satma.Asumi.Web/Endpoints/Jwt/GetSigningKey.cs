using Microsoft.AspNetCore.Mvc;
using Satma.Asumi.Web.Services;

namespace Satma.Asumi.Web.Endpoints.Jwt;

[ApiController]
public class GetSigningKeyController(JwtSigningKeyService jwtSigningKeyService) : ControllerBase
{
    [HttpGet("/api/jwt/signing-key")]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    public IActionResult GetSigningKey()
    {
        var signingKeyInPemFormat = jwtSigningKeyService.SigningKey.ExportRSAPublicKeyPem();
        return Ok(signingKeyInPemFormat);
    }
}
