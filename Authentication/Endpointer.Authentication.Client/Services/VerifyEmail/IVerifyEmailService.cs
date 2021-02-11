using Endpointer.Authentication.Core.Models.Requests;
using Refit;
using System;
using Endpointer.Core.Client.Exceptions;
using System.Collections.Generic;
using Endpointer.Authentication.Client.Exceptions;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services.VerifyEmail
{
    public interface IVerifyEmailService
    {
        /// <summary>
        /// Verify a user's email.
        /// </summary>
        /// <param name="request">The request containing the verification token.</param>
        /// <exception cref="UnauthorizedException">Thrown if user has invalid verification token.</exception>
        /// <exception cref="ValidationFailedException">Thrown if request has validation errors.</exception>
        /// <exception cref="Exception">Thrown if verification fails.</exception>
        Task VerifyEmail(VerifyEmailRequest request);
    }
}
