using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        [SetUp]
        public void SetUp()
        {
            _validModelState = new ModelStateDictionary();
            _invalidModelState = new ModelStateDictionary();
            _invalidModelState.AddModelError("Error", "Message");
        }
    }
}
