using System;
using System.Collections.Generic;
using System.Text;

namespace Endpointer.Authentication.Core.Models.Responses
{
    public class SuccessResponse<T>
    {
        public T Data { get; }

        public SuccessResponse(T data)
        {
            Data = data;
        }
    }
}
