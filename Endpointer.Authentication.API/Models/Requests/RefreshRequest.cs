using System.ComponentModel.DataAnnotations;

namespace Endpointer.Authentication.API.Models.Requests
{
    public class RefreshRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
