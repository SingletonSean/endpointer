using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Endpointer.API.Tests.EndpointHandlers
{
    [TestFixture]
    public class EndpointHandlerTests
    {
        protected ModelStateDictionary _validModelState;
        protected ModelStateDictionary _invalidModelState;

        protected HttpRequest _httpRequest;

        [SetUp]
        public void SetUpBase()
        {
            _validModelState = new ModelStateDictionary();
            _invalidModelState = new ModelStateDictionary();
            _invalidModelState.AddModelError("Error", "Message");

            _httpRequest = new Mock<HttpRequest>().Object;
        }
    }
}
