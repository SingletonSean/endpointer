using Endpointer.Core.Models.Responses;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace Endpointer.Core.API.Extensions
{
    public static class ModelStateDictionaryExtensions
    {
        /// <summary>
        /// Create an error response for invalid model state.
        /// </summary>
        /// <param name="modelState">The model state to convert.</param>
        /// <returns>The error response for the invalid model state.</returns>
        public static ErrorResponse CreateErrorResponse(this ModelStateDictionary modelState)
        {
            IEnumerable<ErrorMessageResponse> errorMessages = modelState.Values
                .SelectMany(v => v.Errors.Select(e => new ErrorMessageResponse(ErrorCode.VALIDATION_FAILURE, e.ErrorMessage)));

            return new ErrorResponse(errorMessages);
        }
    }
}
