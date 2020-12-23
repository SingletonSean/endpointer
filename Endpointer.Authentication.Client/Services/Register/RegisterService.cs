using Endpointer.Authentication.Client.Exceptions;
using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Authentication.Core.Models.Responses;
using Endpointer.Core.Models.Responses;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services.Register
{
    public class RegisterService : IRegisterService
    {
        private readonly IAPIRegisterService _api;

        public RegisterService(IAPIRegisterService api)
        {
            _api = api;
        }

        /// <inheritdoc/>
        public async Task Register(RegisterRequest request)
        {
            try
            {
                await _api.Register(request);
            }
            catch (ApiException ex)
            {
                ErrorResponse error = await ex.GetContentAsAsync<ErrorResponse>();
                if (error == null || error.Errors == null || error.Errors.Count() == 0) 
                    throw;

                ErrorMessageResponse firstError = error.Errors.First();
                switch(firstError.Code)
                {
                    case ErrorCode.PASSWORDS_DO_NOT_MATCH:
                        throw new ConfirmPasswordException();
                    case ErrorCode.EMAIL_ALREADY_EXISTS:
                        throw new EmailAlreadyExistsException(request.Email);
                    case ErrorCode.USERNAME_ALREADY_EXISTS:
                        throw new UsernameAlreadyExistsException(request.Username);
                    case ErrorCode.VALIDATION_FAILURE:
                        throw new ValidationException(error.Errors
                            .Where(e => e.Code == ErrorCode.VALIDATION_FAILURE)
                            .Select(e => e.Message));
                    default:
                        throw;
                }
            }
        }
    }
}
