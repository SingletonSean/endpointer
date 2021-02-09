using FluentEmail.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Services.EmailSenders
{
    public class FluentEmailSender : IEmailSender
    {
        /// <inheritdoc />
        public async Task Send(string from, string to, string subject, string body)
        {
            await Email.From(from).To(to).Subject(subject).Body(body).SendAsync();
        }
    }
}
