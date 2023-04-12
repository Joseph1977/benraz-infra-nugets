using Microsoft.Extensions.Options;
using NUnit.Framework;
using Benraz.Infrastructure.Common.Emails;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Common.Tests
{
    [TestFixture]
    [Ignore("EmailService integration tests.")]
    public class EmailServiceTests
    {
        private const string EMAIL = "address@gmail.com";
        private const string PASSWORD = "password";

        private EmailService _emailService;

        [SetUp]
        public void SetUp()
        {
            var emailServiceSettings = new EmailServiceSettings
            {
                SmtpHostName = "smtp.gmail.com",
                SmtpPort = 25,
                Login = EMAIL,
                Password = PASSWORD,
                IsSslEnabled = true
            };
            _emailService = new EmailService(Options.Create(emailServiceSettings));
        }

        [Test]
        public async Task SendEmailAsync_CorrectEmail_SendsEmail()
        {
            await _emailService.SendEmailAsync(EMAIL, new string[] { EMAIL }, "Test subject", "Test body");
        }
    }
}




