using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace Endpointer.Authentication.API.Models.Responses
{
    public class ErrorResponse
    {
        public IEnumerable<string> ErrorMessages { get; set; }

        public ErrorResponse(string errorMessage) : this(new List<string>() { errorMessage }) { }

        public ErrorResponse(IEnumerable<string> errorMessages)
        {
            ErrorMessages = errorMessages;
        }

        public ErrorResponse(ModelStateDictionary modelState)
        {
            ErrorMessages = modelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
        }
    }
}
