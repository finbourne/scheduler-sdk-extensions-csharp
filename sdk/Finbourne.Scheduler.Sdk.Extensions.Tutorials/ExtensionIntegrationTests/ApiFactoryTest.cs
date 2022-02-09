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

        [Test]
        public void Create_ApplicationMetadataApi()
        {
            var api = _factory.Api<ApplicationMetadataApi>();

            Assert.That(api, Is.Not.Null);
            Assert.That(api, Is.InstanceOf<ApplicationMetadataApi>());
        }

        [Test]
        public void Create_ImagesApi()
        {
            var api = _factory.Api<ImagesApi>();

            Assert.That(api, Is.Not.Null);
            Assert.That(api, Is.InstanceOf<ImagesApi>());
        }

        [Test]
        public void Create_JobsApi()
        {
            var api = _factory.Api<JobsApi>();

            Assert.That(api, Is.Not.Null);
            Assert.That(api, Is.InstanceOf<JobsApi>());
        }

        [Test]
        public void Create_SchedulesApi()
        {
            var api = _factory.Api<SchedulesApi>();

            Assert.That(api, Is.Not.Null);
            Assert.That(api, Is.InstanceOf<SchedulesApi>());
        }

        [Test]
        public void Api_From_Interface()
        {
            var api = _factory.Api<ISchedulesApi>();

            Assert.That(api, Is.Not.Null);
            Assert.That(api, Is.InstanceOf<SchedulesApi>());
        }

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
