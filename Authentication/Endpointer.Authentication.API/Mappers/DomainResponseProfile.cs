using AutoMapper;
using Endpointer.Authentication.API.Models;
using Endpointer.Core.Models.Responses;

namespace Endpointer.Authentication.API.Mappers
{
    /// <summary>
    /// Profile to map domain objects to API response objects.
    /// </summary>
    public class DomainResponseProfile : Profile
    {
        public DomainResponseProfile()
        {
            CreateMap<AuthenticatedUser, AuthenticatedUserResponse>();
        }
    }
}
