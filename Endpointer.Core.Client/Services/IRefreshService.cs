using Endpointer.Core.Models.Requests;
using Endpointer.Core.Models.Responses;
using Refit;
using System.Threading.Tasks;

namespace Endpointer.Core.Client.Services
{
    public interface IRefreshService
    {
        [Post("/")]
        Task<SuccessResponse<AuthenticatedUserResponse>> Refresh([Body] RefreshRequest request);
    }
}
