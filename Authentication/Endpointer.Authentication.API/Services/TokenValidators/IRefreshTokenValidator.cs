namespace Endpointer.Authentication.API.Services.TokenValidators
{
    public interface IRefreshTokenValidator
    {
        /// <summary>
        /// Validate a JWT refresh token.
        /// </summary>
        /// <param name="refreshToken">The token to validate.</param>
        /// <returns>True/false for is valid.</returns>
        bool Validate(string refreshToken);
    }
}