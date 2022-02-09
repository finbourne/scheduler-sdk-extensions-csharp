using Finbourne.Scheduler.Sdk.Api;
using Finbourne.Scheduler.Sdk.Client;
using Finbourne.Scheduler.Sdk.Model;
using NUnit.Framework;
using System;

namespace Finbourne.Scheduler.Sdk.Extensions.Tutorials
{
    public class ApiFactoryTest
    {
        private IApiFactory _factory;

        [OneTimeSetUp]
        public void SetUp()
        {
            _factory = IntegrationTestApiFactoryBuilder.CreateApiFactory("secrets.json");
        }

        /* Add this test for each Api within Finbourne.Scheduler.Sdk.Api
        [Test]
        public void Create_XXXApi()
        {
            var api = _factory.Api<XXXApi>();

            Assert.That(api, Is.Not.Null);
            Assert.That(api, Is.InstanceOf<XXXApi>());
        }
        */

        /* Add this test for and interface of an Api within Finbourne.Scheduler.Sdk.Api
        [Test]
        public void Api_From_Interface()
        {
            var api = _factory.Api<IXXXApi>();

            Assert.That(api, Is.Not.Null);
            Assert.That(api, Is.InstanceOf<IXXXApi>());
        }
        */

        [Test]
        public void Invalid_Requested_Api_Throws()
        {
            Assert.That(() => _factory.Api<InvalidApi>(), Throws.TypeOf<InvalidOperationException>());
        }

        class InvalidApi : IApiAccessor
        {
            public IReadableConfiguration Configuration { get; set; }
            public string GetBasePath()
            {
                throw new NotImplementedException();
            }

            public ExceptionFactory ExceptionFactory { get; set; }
        }
    }
}
