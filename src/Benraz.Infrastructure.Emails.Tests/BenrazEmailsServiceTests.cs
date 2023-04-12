using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Benraz.Infrastructure.Emails.BenrazEmailsService;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Emails.Tests
{
    [TestFixture]
    [Ignore("Integration tests.")]
    public class BenrazEmailsServiceTests
    {
        private const string ACCESS_TOKEN = "";

        private BenrazEmailsService.BenrazEmailsService _BenrazEmailsService;

        [SetUp]
        public void SetUp()
        {
            var emailSettings = new BenrazEmailsServiceSettings
            {
                AccessToken = ACCESS_TOKEN,
                TemplateId = "*****TBD******",
                BaseUrl = "http://localhost:59456"
            };

            var logger = Mock.Of<ILogger<BenrazEmailsService.BenrazEmailsService>>();

            _BenrazEmailsService = new BenrazEmailsService.BenrazEmailsService(
                null, logger, Options.Create(emailSettings));
        }

        [Test]
        public async Task SendEmailAsync_CorrectEmailData_NoError()
        {
            var message = new MailMessage
            {
                From = new MailAddress("info@*****TBD******.com", "Benraz"),
                Subject = "Test email",
                Body = Emails.CustomHtml
            };
            message.To.Add("yourEmail@*****TBD******.com");

            await _BenrazEmailsService.SendEmailAsync(message);
        }
    }
}



