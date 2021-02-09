using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Services.EmailSenders
{
    public interface IEmailSender
    {
        /// <summary>
        /// Send an email.
        /// </summary>
        /// <param name="fromAddress">The email sender.</param>
        /// <param name="toAddress">The email destination.</param>
        /// <param name="subject">The email subject.</param>
        /// <param name="body">The email body content.</param>
        /// <exception cref="Exception">Thrown if email fails to send.</exception>
        Task Send(string fromAddress, string toAddress, string subject, string body);
    }
}
