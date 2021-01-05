using Refit;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Tests.Services
{
    public class APIServiceTests
    {
        protected async Task<ApiException> CreateApiException(
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError,
            object content = null)
        {
            return await ApiException.Create(null,
                HttpMethod.Post,
                new HttpResponseMessage(statusCode)
                {
                    Content = JsonContent.Create(content),
                },
                new RefitSettings(new NewtonsoftJsonContentSerializer()));
        }
    }
}
