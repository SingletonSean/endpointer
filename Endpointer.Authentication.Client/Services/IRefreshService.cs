using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Authentication.Core.Models.Responses;
using Refit;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services
{
    public interface IRefreshService
    {
        [Post("/")]
        Task<SuccessResponse<AuthenticatedUserResponse>> Refresh([Body] RefreshRequest request);
    }
}
