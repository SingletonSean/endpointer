using System.ComponentModel.DataAnnotations;

namespace Endpointer.Authentication.Core.Models.Requests
{
    /// <summary>
    /// Model for a login request.
    /// </summary>
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
