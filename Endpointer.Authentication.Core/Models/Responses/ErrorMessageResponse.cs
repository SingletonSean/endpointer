using System;
using System.Collections.Generic;
using System.Text;

namespace Endpointer.Authentication.Core.Models.Responses
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
