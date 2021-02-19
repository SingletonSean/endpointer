using Endpointer.Core.API.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Services.EmailVerificationSenders
{
    public interface IEmailVerificationSender
    {
        /// <summary>
        /// Send an email verification email to a user.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <exception cref="Exception">Thrown if email sending fails.</exception>
        Task SendEmailVerificationEmail(User user);
    }
}
