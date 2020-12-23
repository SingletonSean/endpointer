using Endpointer.Authentication.Core.Models.Requests;
using Refit;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services.Register
{
    public interface IAPIRegisterService
    {
        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="request">The request containing the register information.</param>
        /// <exception cref="ApiException">Thrown if request fails.</returns>
        [Post("/")]
        Task Register([Body] RegisterRequest request);
    }
}
