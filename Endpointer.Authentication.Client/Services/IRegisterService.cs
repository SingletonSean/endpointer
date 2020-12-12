using Endpointer.Authentication.Core.Models.Requests;
using Refit;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services
{
    public interface IRegisterService
    {
        [Post("/")]
        Task Register([Body] RegisterRequest request);
    }
}
