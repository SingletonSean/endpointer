namespace Endpointer.Core.Models.Responses
{
    public class ErrorMessageResponse
    {
        public int Code { get; }
        public string Message { get; }

        public ErrorMessageResponse(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
