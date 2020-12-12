using Endpointer.Authentication.Core.Models.Requests;
using Refit;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services
{
    public interface IRefreshService
    {
        [Post("/")]
        Task Refresh([Body] RefreshRequest request);
    }
}
