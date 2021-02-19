using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Endpointer.API.Tests.ServiceCollections
{
    [TestFixture]
    public class MockServiceCollectionTests
    {
        private Mock<IServiceCollection> _mockServices;

        public Mock<IServiceCollection> MockServices => _mockServices;

        [SetUp]
        public void SetUp()
        {
            _mockServices = new Mock<IServiceCollection>();
            _mockServices.Setup(s => s.GetEnumerator()).Returns(new List<ServiceDescriptor>().GetEnumerator());
        }

        public void VerifyServiceAdded(Type inter, Type implem)
        {
            _mockServices.Verify(s => s.Add(It.Is<ServiceDescriptor>(s => s.ServiceType == inter && s.ImplementationType == implem)));
        }

        public void VerifyServiceAdded(Type inter, object implem)
        {
            _mockServices.Verify(s => s.Add(It.Is<ServiceDescriptor>(s => s.ServiceType == inter && s.ImplementationInstance == implem)));
        }
    }
}
