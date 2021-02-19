using Endpointer.Authentication.Core.Models.Requests;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services.VerifyEmail
{
    public interface IAPIVerifyEmailService
    {
        /// <summary>
        /// Verify a user's email.
        /// </summary>
        /// <param name="request">The request containing the verification token.</param>
        /// <exception cref="ApiException">Thrown if request fails.</returns>
        [Post("/")]
        Task VerifyEmail([Body] VerifyEmailRequest request);
    }
}
