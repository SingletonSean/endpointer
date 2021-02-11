using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Core.Client.Exceptions;
using Microsoft.Extensions.Logging;
using Refit;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services.VerifyEmail
{
    public class VerifyEmailService : IVerifyEmailService
    {
        private readonly IAPIVerifyEmailService _api;
        private readonly ILogger<VerifyEmailService> _logger;

        public VerifyEmailService(IAPIVerifyEmailService api, ILogger<VerifyEmailService> logger)
        {
            _api = api;
            _logger = logger;
        }

        ///<inheritdoc />
        public async Task VerifyEmail(VerifyEmailRequest request)
        {
            try
            {
                _logger.LogInformation("Sending email verification request.");
                await _api.VerifyEmail(request);

                _logger.LogInformation("Successfully verified user email.");

            }
            catch (ApiException ex)
            {
                if(ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _logger.LogError("Unauthorized email verification token.", ex);
                    throw new UnauthorizedException(ex.Message, ex);
                }

                if(ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    _logger.LogError("Bad email verification request.", ex);
                    throw new ValidationFailedException(ex.Message, ex);
                }

                _logger.LogError("Unknown email verification error.", ex);
                throw;
            }
        }
    }
}
