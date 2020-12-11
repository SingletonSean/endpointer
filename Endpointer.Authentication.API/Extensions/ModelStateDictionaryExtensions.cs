using Endpointer.Authentication.Core.Models.Responses;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Endpointer.Authentication.API.Extensions
{
    public static class ModelStateDictionaryExtensions
    {
        public static ErrorResponse CreateErrorResponse(this ModelStateDictionary modelState)
        {
            IEnumerable<string> errorMessages = modelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));

            return new ErrorResponse(errorMessages);
        }
    }
}
