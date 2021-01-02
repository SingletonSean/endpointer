using AutoMapper;
using Endpointer.Authentication.API.Models;
using Endpointer.Core.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Endpointer.Authentication.API.Mappers
{
    public class DomainResponseProfile : Profile
    {
        public DomainResponseProfile()
        {
            CreateMap<AuthenticatedUser, AuthenticatedUserResponse>();
        }
    }
}
