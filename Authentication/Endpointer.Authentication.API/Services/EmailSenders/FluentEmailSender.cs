using FluentEmail.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Services.EmailSenders
{
    public class FluentEmailSender : IEmailSender
    {
        private readonly IFluentEmail _fluentEmail;

        public FluentEmailSender(IFluentEmail fluentEmail)
        {
            _fluentEmail = fluentEmail;
        }

        /// <inheritdoc />
        public async Task Send(string from, string to, string subject, string body)
        {
            await _fluentEmail.SetFrom(from).To(to).Subject(subject).Body(body).SendAsync();
        }
    }
}
