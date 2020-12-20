using System;
using System.Threading.Tasks;

namespace Endpointer.Core.Client.Stores
{
    public interface IAccessTokenStore
    {
        string AccessToken { get; }
    }
}
