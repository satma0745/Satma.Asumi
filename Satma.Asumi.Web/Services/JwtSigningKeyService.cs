using System.Security.Cryptography;

namespace Satma.Asumi.Web.Services;

public class JwtSigningKeyService
{
    public RSA SigningKey { get; private set; }

    public JwtSigningKeyService()
    {
        SigningKey = RSA.Create();
    }
}