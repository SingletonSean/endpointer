using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Core.Models.Responses;
using Refit;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services
{
    public interface ILoginService
    {
        [Post("/")]
        Task<SuccessResponse<AuthenticatedUserResponse>> Login([Body] LoginRequest request);
    }
}
