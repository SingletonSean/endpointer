using System;
using Endpointer.Core.Client.Exceptions;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services.SendVerifyEmail
{
    public interface ISendVerifyEmailService
    {
        /// <summary>
        /// Send an email verification email request.
        /// </summary>
        /// <exception cref="UnauthorizedException">Thrown if user has invalid access token.</exception>
        /// <exception cref="Exception">Thrown if request fails.</exception>
        Task SendVerifyEmail();
    }
}
