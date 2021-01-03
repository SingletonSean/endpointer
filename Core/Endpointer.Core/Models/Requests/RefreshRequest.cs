using System.ComponentModel.DataAnnotations;

namespace Endpointer.Core.Models.Requests
{
    public class RefreshRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
