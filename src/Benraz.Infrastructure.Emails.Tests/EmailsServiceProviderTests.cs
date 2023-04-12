using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Benraz.Infrastructure.Emails.BenrazEmailsService;
using System;

namespace Benraz.Infrastructure.Emails.Tests
{
    [TestFixture]
    public class EmailsServiceProviderTests
    {
        [Test]
        public void GetService_Benraz_ReturnsBenrazEmailsService()
        {
            var provider = CreateProvider(EmailsServiceType.Benraz);

            var service = provider.GetService();

            service.Should().BeOfType<BenrazEmailsService.BenrazEmailsService>();
        }

        [Test]
        public void GetService_NotSupportedFilesType_ThrowsNotSupportedException()
        {
            var provider = CreateProvider(0);

            provider.Invoking(x => x.GetService()).Should().Throw<NotSupportedException>();
        }

        private EmailsServiceProvider CreateProvider(EmailsServiceType emailServiceType)
        {
            var settings = new EmailsServiceProviderSettings
            {
                ServiceType = emailServiceType,
            };

            settings.Benraz = new BenrazEmailsServiceSettings
            {
                TemplateId = "SendGriidTemplateeGuid",
                BaseUrl = "http://localhost:59456"
            };

            var logger = Mock.Of<ILogger<BenrazEmailsService.BenrazEmailsService>>();

            return new EmailsServiceProvider(null, logger, Options.Create(settings));
        }
    }
}



