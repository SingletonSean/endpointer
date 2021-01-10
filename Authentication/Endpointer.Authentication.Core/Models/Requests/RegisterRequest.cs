using System.ComponentModel.DataAnnotations;

namespace Endpointer.Authentication.Core.Models.Requests
{
    /// <summary>
    /// Model for a register request.
    /// </summary>
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}
