using FluentEmail.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Services.EmailSenders
{
    public class FluentEmailSender : IEmailSender
    {
        private readonly IFluentEmail _fluentEmail;
        private readonly ILogger<FluentEmailSender> _logger;

        public FluentEmailSender(IFluentEmail fluentEmail, ILogger<FluentEmailSender> logger)
        {
            _fluentEmail = fluentEmail;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task Send(string fromAddress, string fromName, string to, string subject, string body)
        {
            _logger.LogInformation("Sending email.");
            await _fluentEmail
                .SetFrom(fromAddress, fromName)
                .To(to)
                .Subject(subject)
                .Body(body)
                .SendAsync();
            
            _logger.LogInformation("Successfully sent email.");
        }
    }
}
