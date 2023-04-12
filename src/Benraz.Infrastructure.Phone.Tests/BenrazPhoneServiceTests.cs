using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Benraz.Infrastructure.Phone.BenrazPhoneService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Phone.Tests
{
    [TestFixture]
    [Ignore("Integration tests.")]
    public class BenrazPhoneServiceTests
    {
        private BenrazPhoneService.BenrazPhoneService _BenrazPhonesService;

        [SetUp]
        public void SetUp()
        {
            var phoneSettings = new BenrazPhoneServiceSettings
            {
                AccountSID = "*****TBD******",
                AuthToken = "*****TBD******",
                OutTwillionumber = "+*****TBD******",
                BaseUrl = "http://localhost:59456"
            };

            var logger = Mock.Of<ILogger<BenrazPhoneService.BenrazPhoneService>>();

            _BenrazPhonesService = new BenrazPhoneService.BenrazPhoneService(
                null, logger, Options.Create(phoneSettings));
        }

        [Test]
        public async Task SendSmsAsync_CorrectSmsData_NoError()
        {
            var sender = "+*****TBD******";
            var recipient = new List<string> { "+*****TBD******" };
            var body = "test message";

            await _BenrazPhonesService.SendSmsAsync(sender, recipient, body);
        }
    }
}


