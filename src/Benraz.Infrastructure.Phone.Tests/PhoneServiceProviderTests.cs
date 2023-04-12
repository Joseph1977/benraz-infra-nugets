using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using FluentAssertions;
using Benraz.Infrastructure.Phone.BenrazPhoneService;
using Microsoft.Extensions.Options;

namespace Benraz.Infrastructure.Phone.Tests
{
    [TestFixture]
    public class PhoneServiceProviderTests
    {
        [Test]
        public void GetService_Benraz_ReturnsBenrazPhoneService()
        {
            var provider = CreateProvider(PhoneServiceType.Benraz);

            var service = provider.GetService();

            service.Should().BeOfType<BenrazPhoneService.BenrazPhoneService>();
        }

        [Test]
        public void GetService_NotSupportedFilesType_ThrowsNotSupportedException()
        {
            var provider = CreateProvider(0);

            provider.Invoking(x => x.GetService()).Should().Throw<NotSupportedException>();
        }

        private PhoneServiceProvider CreateProvider(PhoneServiceType phoneServiceType)
        {
            var settings = new PhoneServiceProviderSettings
            {
                ServiceType = phoneServiceType,
            };

            settings.Benraz = new BenrazPhoneServiceSettings
            {
                AccountSID = "*****TBD******",
                AuthToken = "*****TBD******",
                OutTwillionumber = "+*****TBD******",
                BaseUrl = "http://localhost:59456"
            };

            var logger = Mock.Of<ILogger<BenrazPhoneService.BenrazPhoneService>>();

            return new PhoneServiceProvider(null, logger, Options.Create(settings));
        }
    }
}


