using NUnit.Framework;
using System;

namespace Finbourne.Scheduler.Sdk.Extensions.Tests
{
    [TestFixture]
    public class ApiFactoryTest
    {
        [Test]
        public void InvalidTokenUrl_ThrowsException()
        {
            ApiConfiguration apiConfig = new ApiConfiguration
            {
                TokenUrl = "xyz"
            };

            Assert.That(
                () => new ApiFactory(apiConfig),
                Throws.InstanceOf<UriFormatException>().With.Message.EqualTo("Invalid Token Uri: xyz"));
        }

        [Test]
        public void InvalidSchedulerUrl_ThrowsException()
        {
            ApiConfiguration apiConfig = new ApiConfiguration
            {
                TokenUrl = "http://finbourne.com",
                SchedulerUrl = "xyz"
            };

            Assert.That(
                () => new ApiFactory(apiConfig),
                Throws.InstanceOf<UriFormatException>().With.Message.EqualTo("Invalid Uri: xyz"));
        }
    }
}
