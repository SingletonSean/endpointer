using Endpointer.Core.Client.Exceptions;
using Microsoft.Extensions.Logging;
using Refit;
using System.Net;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services.SendVerifyEmail
{
    public class SendVerifyEmailService : ISendVerifyEmailService
    {
        private readonly IAPISendVerifyEmailService _api;
        private readonly ILogger<SendVerifyEmailService> _logger;

        public SendVerifyEmailService(IAPISendVerifyEmailService api, ILogger<SendVerifyEmailService> logger)
        {
            _api = api;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task SendVerifyEmail()
        {
            try
            {
                _logger.LogInformation("Sending email verification email request.");
                await _api.SendVerifyEmail();

                _logger.LogInformation("Successfully requested email verification email.");
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, "Email verification email request failed.");
                if (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _logger.LogError("Unauthorized request.");
                    throw new UnauthorizedException(ex.Message, ex);
                }

                _logger.LogError("Unknown error.");
                throw;
            }
        }
    }
}
