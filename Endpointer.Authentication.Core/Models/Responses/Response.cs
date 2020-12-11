using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Endpointer.Authentication.Core.Models.Responses
{
    public class Response
    {
        public bool Success => Errors?.Count() == 0;

        public IEnumerable<ErrorMessageResponse> Errors { get; set; }
    }

    public class Response<T> : Response
    {
        public T Data { get; set; }
    }
}
