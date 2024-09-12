using BcryptNet = BCrypt.Net.BCrypt;

namespace Satma.Asumi.Web.Services;

public class PasswordService
{
    public string HashPassword(string plainTextPassword)
    {
        return BcryptNet.EnhancedHashPassword(plainTextPassword);
    }

    public bool DoPasswordsMatch(string plainTextPassword, string hashedPassword)
    {
        return BcryptNet.EnhancedVerify(plainTextPassword, hashedPassword);
    }
}