using MissionFlow.Application.Common.Interfaces;
using System.Security.Cryptography;

namespace MissionFlow.Infrastructure.Auth;

public sealed class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100000;

    public string Hash(string password)
    {
        var salt = new byte[SaltSize];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            HashSize);

        return $"{Convert.ToHexString(hash)}.{Convert.ToHexString(salt)}";
    }

    public bool Verify(string password, string hash)
    {
        var parts = hash.Split('.');
        var hashBytes = Convert.FromHexString(parts[0]);
        var saltBytes = Convert.FromHexString(parts[1]);

        var computedHash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            saltBytes,
            Iterations,
            HashAlgorithmName.SHA256,
            HashSize);

        return CryptographicOperations.FixedTimeEquals(hashBytes, computedHash);
    }
}
