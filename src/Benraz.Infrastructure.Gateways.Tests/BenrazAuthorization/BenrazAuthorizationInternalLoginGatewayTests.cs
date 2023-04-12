using FluentAssertions;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Benraz.Infrastructure.Gateways.BenrazAuthorization.InternalLogin;
using Benraz.Infrastructure.Gateways.BenrazAuthorization.InternalLogin.Entities;
using Benraz.Infrastructure.Gateways.BenrazAuthorization.InternalLogin.Messages;
using System.Net;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Gateways.Tests.BenrazAuthorization
{
    [TestFixture]
#if !DEBUG
    [Ignore("BenrazAuthorizationInternalLoginGateway integration tests.")]
#endif
    public class BenrazAuthorizationInternalLoginGatewayTests
    {
        private const string ACCESS_TOKEN = "";

        private IBenrazAuthorizationInternalLoginGateway _gateway;

        [SetUp]
        public void SetUp()
        {
            var settings = new BenrazAuthorizationInternalLoginGatewaySettings()
            {
                BaseUrl = "http://localhost:60341"
            };

            _gateway = new BenrazAuthorizationInternalLoginGateway(
                GatewayTestUtils.CreateHttpClientFactory(), Options.Create(settings));
        }

        [Test]
        public async Task SendAsync_SendConfirmationEmailRequest_ReturnsResponse()
        {
            var request = new SendConfirmationEmailRequest()
            {
                AccessToken = ACCESS_TOKEN,
                User = new SendConfirmationEmail() { UserId = "5fdac8dc-dd0f-4a20-8b86-9b8075ca3b1b" }
            };

            var response = await _gateway.SendAsync(request);

            response.Should().NotBeNull();
            response.IsSuccessHttpStatusCode.Should().BeTrue();
            response.HttpStatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        [Test]
        public async Task SendAsync_RestorePasswordRequest_ReturnsResponse()
        {
            var request = new RestorePasswordRequest()
            {
                AccessToken = ACCESS_TOKEN,
                User = new RestorePassword() { UserName = "user@example.com" }
            };

            var response = await _gateway.SendAsync(request);

            response.Should().NotBeNull();
            response.IsSuccessHttpStatusCode.Should().BeTrue();
            response.HttpStatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }
    }
}



