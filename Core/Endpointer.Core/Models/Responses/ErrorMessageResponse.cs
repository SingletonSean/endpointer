namespace Endpointer.Core.Models.Responses
{
    /// <summary>
    /// Model for an Endpointer error message response.
    /// </summary>
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
