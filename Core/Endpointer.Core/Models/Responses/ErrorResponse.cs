using System.Collections.Generic;

namespace Endpointer.Core.Models.Responses
{
    /// <summary>
    /// Model for an error response. This is the root response object for API errors. 
    /// </summary>
    public class ErrorResponse
    {
        public IEnumerable<ErrorMessageResponse> Errors { get; set; }

        public ErrorResponse() { }

        public ErrorResponse(int code, string errorMessage) : this(new ErrorMessageResponse(code, errorMessage)) { }

        public ErrorResponse(ErrorMessageResponse errorMessage) : this(new List<ErrorMessageResponse>() { errorMessage }) { }

        public ErrorResponse(IEnumerable<ErrorMessageResponse> errors)
        {
            Errors = errors;
        }
    }
}
