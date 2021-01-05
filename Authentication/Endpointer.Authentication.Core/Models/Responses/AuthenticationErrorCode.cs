namespace Endpointer.Authentication.Core.Models.Responses
{
    /// <summary>
    /// Endpointer authentication error codes.
    /// </summary>
    public class AuthenticationErrorCode
    {
        public const int EMAIL_ALREADY_EXISTS = 3;
        public const int USERNAME_ALREADY_EXISTS = 4;
        public const int PASSWORDS_DO_NOT_MATCH = 5;
    }
}
