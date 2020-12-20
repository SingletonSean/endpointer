﻿using Endpointer.Authentication.Core.Models.Responses;
using Endpointer.Core.Models.Responses;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace Endpointer.Authentication.API.Extensions
{
    public static class ModelStateDictionaryExtensions
    {
        public static ErrorResponse CreateErrorResponse(this ModelStateDictionary modelState)
        {
            IEnumerable<ErrorMessageResponse> errorMessages = modelState.Values
                .SelectMany(v => v.Errors.Select(e => new ErrorMessageResponse(ErrorCode.VALIDATION_FAILURE, e.ErrorMessage)));

            return new ErrorResponse(errorMessages);
        }
    }
}
