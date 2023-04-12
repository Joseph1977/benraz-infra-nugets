using FluentAssertions;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Benraz.Infrastructure.Gateways.BenrazAuthorization.Users;
using Benraz.Infrastructure.Gateways.BenrazAuthorization.Users.Messages;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Gateways.Tests.BenrazAuthorization
{
    [TestFixture]
#if !DEBUG
    [Ignore("BenrazAuthorizationUsersGateway integration tests.")]
#endif
    public class BenrazAuthorizationUsersGatewayTests
    {
        private const string ACCESS_TOKEN = "";

        private BenrazAuthorizationUsersGateway _gateway;

        [SetUp]
        public void SetUp()
        {
            var settings = new BenrazAuthorizationUsersGatewaySettings
            {
                BaseUrl = "http://localhost:60341"
            };

            _gateway = new BenrazAuthorizationUsersGateway(
                GatewayTestUtils.CreateHttpClientFactory(), Options.Create(settings));
        }

        [Test]
        public async Task SendAsync_GetUsersRequest_ReturnsResponse()
        {
            var request = new GetUsersRequest
            {
                AccessToken = ACCESS_TOKEN,
                Email = "hellorented+user001@test.org",
                EmailConfirmed = true,
                StatusIds = new int[] { 1, 2 },
                PageNo = 1,
                PageSize = 100
            };

            var response = await _gateway.SendAsync(request);

            response.Should().NotBeNull();
            response.IsSuccessHttpStatusCode.Should().BeTrue();
            response.Page.Should().NotBeNull();
            response.Page.Items.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task SendAsync_GetUserRequest_ReturnsResponse()
        {
            var request = new GetUserRequest
            {
                AccessToken = ACCESS_TOKEN,
                UserId = "5fdac8dc-dd0f-4a20-8b86-9b8075ca3b1b"
            };

            var response = await _gateway.SendAsync(request);

            response.Should().NotBeNull();
            response.IsSuccessHttpStatusCode.Should().BeTrue();
            response.User.Should().NotBeNull();
        }

        [Test]
        public async Task SendAsync_GetUserInfoByEmailRequest_ReturnsResponse()
        {
            var request = new GetUserInfoByEmailRequest
            {
                AccessToken = ACCESS_TOKEN,
                Email = "newtwi3024b1@yahoo.com"
            };

            var response = await _gateway.SendAsync(request);

            response.Should().NotBeNull();
            response.IsSuccessHttpStatusCode.Should().BeTrue();
            response.User.Should().NotBeNull();
        }

        [Test]
        public async Task SendAsync_CreateUserRequest_ReturnsResponse()
        {
            var request = new CreateUserRequest
            {
                AccessToken = ACCESS_TOKEN,
                Email = "email@dot.com",
                FullName = "FullName",
                Password = "Passw!1ord",
                PhoneNumber = "0993322123",
                Roles = new string[] { "New applicant" }
            };

            var response = await _gateway.SendAsync(request);

            response.Should().NotBeNull();
            response.IsSuccessHttpStatusCode.Should().BeTrue();
            response.UserId.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task SendAsync_GetUserRolesRequest_ReturnsResponse()
        {
            var request = new GetUserRolesRequest
            {
                AccessToken = ACCESS_TOKEN,
                UserId = "30abba29-2de2-4cf1-a4ec-b462c836f4f4"
            };

            var response = await _gateway.SendAsync(request);

            response.Should().NotBeNull();
            response.IsSuccessHttpStatusCode.Should().BeTrue();
            response.Roles.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task SendAsync_SetUserRolesRequest_ReturnsResponse()
        {
            var request = new SetUserRolesRequest
            {
                AccessToken = ACCESS_TOKEN,
                UserId = "30abba29-2de2-4cf1-a4ec-b462c836f4f4",
                Roles = new string[] { "New applicant" }
            };

            var response = await _gateway.SendAsync(request);

            response.Should().NotBeNull();
            response.IsSuccessHttpStatusCode.Should().BeTrue();
        }
    }
}



