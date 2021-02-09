using System;
using System.Collections.Generic;
using System.Text;

namespace Endpointer.Authentication.Core.Models.Requests
{
    /// <summary>
    /// Model for a verify email request.
    /// </summary>
    public class VerifyEmailRequest
    {
        public string VerifyToken { get; set; }
    }
}
