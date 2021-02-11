using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Endpointer.Authentication.Core.Models.Requests
{
    /// <summary>
    /// Model for a verify email request.
    /// </summary>
    public class VerifyEmailRequest
    {
        [Required]
        public string VerifyToken { get; set; }
    }
}
