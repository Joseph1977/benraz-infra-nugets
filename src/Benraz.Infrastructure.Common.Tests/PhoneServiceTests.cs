using Microsoft.Extensions.Options;
using NUnit.Framework;
using Benraz.Infrastructure.Common.Phone;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Common.Tests
{
    [TestFixture]
    public class PhoneServiceTests
    {
        private const string recipient = "+*****TBD******";
        private const string sender = "+*****TBD******";
        private PhoneService _phoneService;

        [SetUp]
        public void SetUp()
        {
            var phoneServiceSettings = new PhoneServiceSettings
            {
                AccountSID = "*****TBD******",
                AuthToken = "*****TBD******",
                OutTwillionumber = sender
            };
            _phoneService = new PhoneService(Options.Create(phoneServiceSettings));
        }

        [Test]
        public async Task SendSmsAsync_ValidNumber()
        {
            await _phoneService.SendSms(sender, new List<string> { recipient }, "test message");
        }
    }
}



