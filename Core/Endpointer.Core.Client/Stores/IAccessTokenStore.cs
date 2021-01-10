namespace Endpointer.Core.Client.Stores
{
    public interface IAccessTokenStore
    {
        /// <summary>
        /// The stored access token.
        /// </summary>
        string AccessToken { get; }
    }
}
