using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Core.Client.Exceptions;
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

        public VerifyEmailService(IAPIVerifyEmailService api)
        {
            _api = api;
        }

        ///<inheritdoc />
        public async Task VerifyEmail(VerifyEmailRequest request)
        {
            try
            {
                await _api.VerifyEmail(request);
            }
            catch (ApiException ex)
            {
                if(ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedException(ex.Message, ex);
                }

                if(ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new ValidationFailedException(ex.Message, ex);
                }

                throw;
            }
        }
    }
}
