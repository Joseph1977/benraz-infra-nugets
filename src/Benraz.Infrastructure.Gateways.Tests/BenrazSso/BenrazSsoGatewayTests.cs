using FluentAssertions;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Benraz.Infrastructure.Gateways.BenrazSso;
using Benraz.Infrastructure.Gateways.BenrazSso.Messages;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Gateways.Tests.BenrazSso
{
    [TestFixture]
    [Ignore("BenrazSsoGateway integration tests.")]
    public class BenrazSsoGatewayTests
    {
        private BenrazSsoGateway _gateway;

        [SetUp]
        public void SetUp()
        {
            var settings = new BenrazSsoGatewaySettings
            {
                BaseUrl = "http://localhost:60341"
            };
            _gateway = new BenrazSsoGateway(
                GatewayTestUtils.CreateHttpClientFactory(), Options.Create(settings));
        }

        [Test]
        public async Task SendAsync_LoginRequest_ReturnsLoginResponse()
        {
            var request = new LoginRequest
            {
                Email = "test041@test.org",
                Password = "password"
            };
            request.SetState("d5069819-1256-4aef-9341-2628237c8ee6");

            var response = await _gateway.SendAsync(request);

            response.Should().NotBeNull();
            response.Error.Should().BeNullOrEmpty();
            response.AccessToken.Should().NotBeNullOrEmpty();
        }
    }
}



