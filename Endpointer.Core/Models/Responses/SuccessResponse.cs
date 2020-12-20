namespace Endpointer.Core.Models.Responses
{
    public class SuccessResponse<T>
    {
        public T Data { get; }

        public SuccessResponse(T data)
        {
            Data = data;
        }
    }
}
