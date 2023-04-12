using FluentAssertions;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Benraz.Infrastructure.Domain.Authorization;
using Benraz.Infrastructure.Gateways.BenrazAuthorization.Auth;
using Benraz.Infrastructure.Gateways.BenrazAuthorization.Auth.Messages;
using System;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Gateways.Tests.BenrazAuthorization
{
    [TestFixture]
    [Ignore("BenrazAuthorizationAuthGateway integration tests.")]
    public class BenrazAuthorizationAuthGatewayTests
    {
        private const string ACCESS_TOKEN = "";

        private BenrazAuthorizationAuthGateway _gateway;

        [SetUp]
        public void SetUp()
        {
            var settings = new BenrazAuthorizationAuthGatewaySettings
            {
                BaseUrl = "http://localhost:60341"
            };

            _gateway = new BenrazAuthorizationAuthGateway(
                GatewayTestUtils.CreateHttpClientFactory(), Options.Create(settings));
        }

        [Test]
        public async Task SendAsync_ParametersRequest_ReturnsParametersResponse()
        {
            var request = new ParametersRequest();

            var response = await _gateway.SendAsync(request);

            response.Should().NotBeNull();
            response.KeySet.Should().NotBeNullOrEmpty();
            response.Issuer.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task SendAsync_IsTokenActiveRequest_ReturnsResponse()
        {
            var request = new IsTokenActiveRequest
            {
                TokenId = Guid.Parse("8E2ECEB5-811F-421D-8BB2-08D7BEF22198")
            };

            var response = await _gateway.SendAsync(request);

            response.Should().NotBeNull();
            response.IsActive.Should().BeTrue();
        }

        [Test]
        public async Task SendAsync_TokenRequest_ReturnsResponse()
        {
            var request = new TokenRequest
            {
                ApplicationId = Guid.Parse("d5069819-1256-4aef-9341-2628237c8ee6"),
                SsoProviderCode = (int)SsoProviderCode.Internal,
                Username = "test041@test.org",
                Password = "Qwerty123!",
                GrantType = "password"
            };

            var response = await _gateway.SendAsync(request);

            response.Should().NotBeNull();
            response.Error.Should().BeNullOrEmpty();
            response.AccessToken.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task SendAsync_ExchangeTokenRequest_ReturnsResponse()
        {
            var request = new ExchangeTokenRequest
            {
                AccessToken = ACCESS_TOKEN,
                ApplicationId = Guid.Parse("F89AAB5C-B98C-4090-5940-08D84F4AE0FC"),
                AccessTokenToExchange = ""
            };

            var response = await _gateway.SendAsync(request);

            response.Should().NotBeNull();
            response.Error.Should().BeNullOrEmpty();
            response.AccessToken.Should().NotBeNullOrEmpty();
        }
    }
}



