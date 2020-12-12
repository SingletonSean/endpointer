using System.Collections.Generic;
using System.Linq;

namespace Endpointer.Authentication.Core.Models.Responses
{
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
