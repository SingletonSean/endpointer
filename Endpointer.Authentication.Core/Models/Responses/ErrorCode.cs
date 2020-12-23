namespace Endpointer.Authentication.Core.Models.Responses
{
    public class ErrorCode
    {
        public const int EMAIL_ALREADY_EXISTS = 1;
        public const int USERNAME_ALREADY_EXISTS = 2;
        public const int PASSWORDS_DO_NOT_MATCH = 3;
        public const int VALIDATION_FAILURE = 4;
        public const int INVALID_REFRESH_TOKEN = 5;
    }
}
