using System.ComponentModel.DataAnnotations;

namespace Endpointer.Core.Models.Requests
{
    /// <summary>
    /// Model for a refresh token request.
    /// </summary>
    public class RefreshRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
