using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Authentication.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services.Register
{
    public interface IRegisterService
    {
        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="request">The request containing the register information.</param>
        /// <exception cref="ConfirmPasswordException">Thrown if password and confirm password do not match.</exception>
        /// <exception cref="EmailAlreadyExistsException">Thrown if email already exists.</exception>
        /// <exception cref="UsernameAlreadyExistsException">Thrown if username already exists.</exception>
        /// <exception cref="ValidationException">Thrown if request has validation errors.</exception>
        /// <exception cref="Exception">Thrown if register fails.</exception>
        Task Register(RegisterRequest request);
    }
}
