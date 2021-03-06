﻿using Refit;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Endpointer.Client.Tests.Services
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
