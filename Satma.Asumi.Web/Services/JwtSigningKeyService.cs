using System.Security.Cryptography;

namespace Satma.Asumi.Web.Services;

public class JwtSigningKeyService
{
    // TODO: Use cache for this key, instead of relying in this service to be singleton.
    public RSA SigningKey { get; private set; }

    public JwtSigningKeyService()
    {
        SigningKey = RSA.Create();
    }
}