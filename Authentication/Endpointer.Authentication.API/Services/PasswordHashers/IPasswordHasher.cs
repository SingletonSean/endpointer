namespace Endpointer.Authentication.API.Services.PasswordHashers
{
    public interface IPasswordHasher
    {
        /// <summary>
        /// Hash a password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>The hashed password.</returns>
        string HashPassword(string password);

        /// <summary>
        /// Verify a password against a password hash.
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <param name="passwordHash">The hash of the password.</param>
        /// <returns>True/false for password matches password hash.</returns>
        bool VerifyPassword(string password, string passwordHash);
    }
}
