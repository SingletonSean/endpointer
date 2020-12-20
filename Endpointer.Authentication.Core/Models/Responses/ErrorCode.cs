namespace Endpointer.Authentication.Core.Models.Responses
{
    public class ErrorCode
    {
        public static int EMAIL_ALREADY_EXISTS = 1;
        public static int USERNAME_ALREADY_EXISTS = 2;
        public static int PASSWORDS_DO_NOT_MATCH = 3;
        public static int VALIDATION_FAILURE = 4;
        public static int INVALID_REFRESH_TOKEN = 5;
    }
}
