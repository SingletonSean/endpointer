using Refit;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services.SendVerifyEmail
{
    public interface IAPISendVerifyEmailService
    {
        /// <summary>
        /// Send an email verification email request.
        /// </summary>
        /// <exception cref="ApiException">Thrown if request fails.</exception>
        [Post("/")]
        Task SendVerifyEmail();
    }
}
