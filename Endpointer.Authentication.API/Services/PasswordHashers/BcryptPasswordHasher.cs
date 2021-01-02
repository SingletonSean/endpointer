namespace Endpointer.Authentication.API.Services.PasswordHashers
{
    public class BcryptPasswordHasher : IPasswordHasher
    {
        /// <inheritdoc />
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <inheritdoc />
        public bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
