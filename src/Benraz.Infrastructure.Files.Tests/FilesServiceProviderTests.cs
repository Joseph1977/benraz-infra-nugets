using FluentAssertions;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Benraz.Infrastructure.Files.Azure;
using Benraz.Infrastructure.Files.FTP;
using Benraz.Infrastructure.Files.Local;
using System;

namespace Benraz.Infrastructure.Files.Tests
{
    [TestFixture]
    public class FilesServiceProviderTests
    {
        [Test]
        public void GetService_LocalFiles_ReturnsLocalFilesService()
        {
            var provider = CreateProvider(FileType.Local);

            var service = provider.GetService();

            service.Should().BeOfType<LocalFilesService>();
        }

        [Test]
        public void GetService_AzureBlobFiles_ReturnsAzureBlobFilesService()
        {
            var provider = CreateProvider(FileType.AzureBlob);

            var service = provider.GetService();

            service.Should().BeOfType<AzureBlobFilesService>();
        }

        [Test]
        public void GetService_FtpFiles_ReturnsFtpFilesService()
        {
            var provider = CreateProvider(FileType.Ftp);

            var service = provider.GetService();

            service.Should().BeOfType<FtpFilesService>();
        }

        [Test]
        public void GetService_NotSupportedFilesType_ThrowsNotSupportedException()
        {
            var provider = CreateProvider(0);

            provider.Invoking(x => x.GetService()).Should().Throw<NotSupportedException>();
        }

        private FilesServiceProvider CreateProvider(FileType filesType)
        {
            var settings = new FilesServiceProviderSettings
            {
                FilesType = filesType
            };

            return new FilesServiceProvider(Options.Create(settings));
        }
    }
}



