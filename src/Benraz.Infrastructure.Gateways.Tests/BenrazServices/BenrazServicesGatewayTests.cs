using FluentAssertions;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Benraz.Infrastructure.Gateways.BenrazCommon;
using Benraz.Infrastructure.Gateways.BenrazServices;
using Benraz.Infrastructure.Gateways.BenrazServices.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Gateways.Tests.BenrazServices
{
    [TestFixture]
    [Ignore("Integration tests.")]
    public class BenrazServicesGatewayTests
    {
        private const string ACCESS_TOKEN = "";

        private BenrazServicesGateway _BenrazServicesGateway;

        [SetUp]
        public void SetUp()
        {
            var settings = new BenrazCommonGatewaySettings
            {
                BaseUrl = "http://localhost:59456"
            };

            _BenrazServicesGateway = new BenrazServicesGateway(
                GatewayTestUtils.CreateHttpClientFactory(), Options.Create(settings));
        }

        [Test]
        public async Task SendAsync_EmailRequest_SendsEmail()
        {
            var request = new EmailV2Request
			{
                AccessToken = ACCESS_TOKEN,
                BasicInfo = new EmailBasicInfo
                {
                    From = "info@org.com",
                    DisplayName = "Joseph Benraz",
                    To = "yourEmail@org.com",
                    Subject = "Reset password",
                    TemplateId = "SendGriidTemplateeGuid",
                    SkipOptOutCheck = true
                },
            };

            request.EmailParams = new Dictionary<string, string>();
            request.EmailParams.Add("-RootBannerUrl-", "https://avatars.githubusercontent.com/u/1216345?s=40&v=4");
            request.EmailParams.Add("-corporateName-", "Benraz");
            request.EmailParams.Add("-userName-", "User Name");
            request.EmailParams.Add("-ActionLink-", "http://localhost:4205/set-password#access_token=123");
            request.EmailParams.Add("-corporateEmail-", "support@org.com");
            request.EmailParams.Add("-corporateNumber-", "1-888-497-00000");

            var response = await _BenrazServicesGateway.SendAsync(request);

            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(200);
        }

        [Test]
        public async Task SendAsync_EmailRequestOnlyTos_SendsEmail()
        {
            var request = new EmailV2Request
            {
                AccessToken = ACCESS_TOKEN,
                BasicInfo = new EmailBasicInfo
                {
                    From = "info@org.com",
                    DisplayName = "Benraz",
                    Tos = new string[] { "yourEmail@org.com" },
                    Subject = "Html email",
                    TemplateId = "SendGriidTemplateeGuid",
                    SkipOptOutCheck = true
                },
                EmailParams = new Dictionary<string, string>
                {
                    {"-html-", "<html><body><p>Test body.</p></body></html>"}
                },
            };

            var response = await _BenrazServicesGateway.SendAsync(request);

            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(200);
        }

        [Test]
        
        public async Task SendAsync_PhoneRequest_SendsSms()
        {
            var request = new PhoneRequest
            {
                Body = "TestMessage",
                Recipients = new List<string> { "+****TBD******" },
                Sender = "+****TBD******"
            };

            var response = await _BenrazServicesGateway.SendSmsAsync(request);

            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(200);
        }
    }
}


