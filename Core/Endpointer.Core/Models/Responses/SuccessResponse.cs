namespace Endpointer.Core.Models.Responses
{
    /// <summary>
    /// Model for a successful response. This is the root response object for API data. 
    /// </summary>
    public class SuccessResponse<T>
    {
        public T Data { get; }

        public SuccessResponse(T data)
        {
            Data = data;
        }
    }
}
