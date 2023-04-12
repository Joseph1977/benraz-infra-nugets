using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Benraz.Infrastructure.Files.Azure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Files.Tests
{
    [TestFixture]
    [Ignore("AzureBlobService integration tests.")]
    public class AzureBlobFilesServiceTests
    {
        private const string CONNECTION_STRING = "DefaultEndpointsProtocol=https;AccountName=publicfilestorages4ddev;AccountKey=*****TBD*****==;EndpointSuffix=core.windows.net";
        private const string BLOB_CONTAINER = "dev-applications-blobfiles";
        private const string FOLDER = "folder001";
        private const string BLOB1_NAME = "testblob001";
        private const string BLOB2_NAME = "folder001/testblob002";
        private const string ROOT_DIRECTORY = "microservicename001";

        private AzureBlobFilesService _azureBlobFilesService;

        [SetUp]
        public async Task SetUpAsync()
        {
            await CreateContainerAsync();
            await ClearContainerAsync();

            var settings = new AzureBlobFilesServiceSettings
            {
                ConnectionString = CONNECTION_STRING,
                BlobContainer = BLOB_CONTAINER,
                RootDirectory = ROOT_DIRECTORY
            };
            _azureBlobFilesService = new AzureBlobFilesService(Options.Create(settings));
        }

        [TearDown]
        public async Task TearDownAsync()
        {
            await ClearContainerAsync();
        }

        [Test]
        public async Task FindAllAsync_OnlyMetadata_ReturnsFilesMetadata()
        {
            await UploadBlobAsync(BLOB1_NAME, "TestBlobContent001");

            var files = await _azureBlobFilesService.FindAllAsync(null, FileProperties.Metadata);

            files.Should().NotBeNullOrEmpty();
            RemoveFirstPathFromRelativeAddressName(files[0].Name).Should().Be(BLOB1_NAME);
            files[0].Content.Should().BeNull();
        }

        [Test]
        public async Task FindAllAsync_AllProperties_ReturnsFiles()
        {
            await UploadBlobAsync(BLOB1_NAME, "TestBlobContent001");

            var files = await _azureBlobFilesService.FindAllAsync(null, FileProperties.All);

            files.Should().NotBeNullOrEmpty();
            RemoveFirstPathFromRelativeAddressName(files[0].Name).Should().Be(BLOB1_NAME);
            files[0].Content.Should().Match(x => Encoding.UTF8.GetString(x.ToArray()) == "TestBlobContent001");
        }

        [Test]
        public async Task FindAllAsync_WithPath_ReturnsFilesMetadata()
        {
            await UploadBlobAsync(BLOB1_NAME, "TestBlobContent001");
            await UploadBlobAsync(BLOB2_NAME, "TestBlobContent002");

            var files = await _azureBlobFilesService.FindAllAsync(FOLDER, FileProperties.Metadata);

            files.Should().NotBeNullOrEmpty();
            RemoveFirstPathFromRelativeAddressName(files[0].Name).Should().Be(BLOB2_NAME);
        }

        [Test]
        public async Task FindByNameAsync_ContainerAndBlobsExist_ReturnsFiles()
        {
            await UploadBlobAsync(BLOB1_NAME, "TestBlobContent001");

            var file = await _azureBlobFilesService.FindByNameAsync(BLOB1_NAME, FileProperties.All);

            file.Should().NotBeNull();
            file.Content.Should().Match(x => Encoding.UTF8.GetString(x.ToArray()) == "TestBlobContent001");
        }

        [Test]
        public async Task CreateAsync_ContainerAndBlobsExist_ReturnsFiles()
        {
            var file = new File
            {
                Name = BLOB1_NAME,
                Content = Encoding.UTF8.GetBytes("TestBlobContent001")
            };
            await _azureBlobFilesService.CreateAsync(file);

            var blobClient = this.GetBlobClient(BLOB1_NAME);
            (await blobClient.ExistsAsync()).Value.Should().BeTrue();
            (await blobClient.DownloadContentAsync()).Value.Content.ToString().Should().Be("TestBlobContent001");
        }

        [Test]
        public async Task RenameAsync_ContainerAndBlobExist_MovesFile()
        {
            await UploadBlobAsync(BLOB1_NAME, "TestBlobContent001");

            var file = new File
            {
                Name = BLOB1_NAME
            };
            await _azureBlobFilesService.RenameAsync(file, BLOB2_NAME);

            var blobClient1 = this.GetBlobClient(BLOB1_NAME);
            var blobClient2 = this.GetBlobClient(BLOB2_NAME);
            (await blobClient1.ExistsAsync()).Value.Should().BeFalse();
            (await blobClient2.ExistsAsync()).Value.Should().BeTrue();
        }

        [Test]
        public async Task DeleteAsync_ContainerAndBlobsExist_DeletesFile()
        {
            await UploadBlobAsync(BLOB1_NAME, "TestBlobContent001");

            await _azureBlobFilesService.DeleteAsync(BLOB1_NAME);

            var blobClient = this.GetBlobClient(BLOB1_NAME);
            (await blobClient.ExistsAsync()).Value.Should().BeFalse();
        }

        [Test]
        public async Task DeleteAllAsync_FromRoot_DeletesFiles()
        {
            await UploadBlobAsync(BLOB1_NAME, "TestBlobContent001");

            await _azureBlobFilesService.DeleteAllAsync(null);

            var blobClient = this.GetBlobClient(BLOB1_NAME);
            (await blobClient.ExistsAsync()).Value.Should().BeFalse();
        }

        [Test]
        public async Task DeletePathAsync_PathExists_DeletesPath()
        {
            await UploadBlobAsync(BLOB2_NAME, "TestBlobContent002");

            await _azureBlobFilesService.DeletePathAsync(FOLDER);

            var blobClient = this.GetBlobClient(BLOB2_NAME);
            (await blobClient.ExistsAsync()).Value.Should().BeFalse();
        }

        [Test]
        public async Task GetUri_CorrectSettings_ReturnsUri()
        {
            await UploadBlobAsync(BLOB1_NAME, "TestBlobContent001");

            var uri = _azureBlobFilesService.GetUri(BLOB1_NAME);

            uri.Should().Be(GetFileUri(BLOB1_NAME));
        }

        [Test]
        public async Task GetSignedUri_CorrectSettings_ReturnsUri()
        {
            await UploadBlobAsync(BLOB1_NAME, "TestBlobContent001");

            var uri = _azureBlobFilesService.GetSignedUri(BLOB1_NAME);

            uri.Should().Be(GetSignedFileUri(BLOB1_NAME));
        }

        [Test]
        public async Task MoveFileAsync_FileAndSourceAndDestination_RemoveFileFromSourceAndCreateOnDestination()
        {

            var sourceFolderName = "sourcefolder";
            var destinationFolderName = "destinationfolder";
            var blobName = "test.txt";
            var sourcePath = GetPathCombine(FOLDER, sourceFolderName);
            var destinationPath = GetPathCombine(FOLDER, destinationFolderName);
            var blobPath = GetPathCombine(sourcePath, blobName);
            var file = new File
            {
                Name = blobPath,
                Content = Encoding.UTF8.GetBytes("TestBlobContent001")
            };
            await _azureBlobFilesService.CreateAsync(file);
            var status = await _azureBlobFilesService.MoveFileAsync(blobName, sourcePath, destinationPath);

            status.Should().BeTrue();

            await _azureBlobFilesService.DeletePathAsync(destinationPath);
        }

        private string GetFullBlobPath(string fileName) => $"{ROOT_DIRECTORY}/{fileName}";

        private async Task UploadBlobAsync(string blobName, string blobContent)
        {
            var blobServiceClient = this.GetBlobServiceClient();
            var containerClient = blobServiceClient.GetBlobContainerClient(BLOB_CONTAINER);

            if (!await containerClient.ExistsAsync())
            {
                throw new ArgumentException("Container hasn't been created.");
            }
            var blobClient = containerClient.GetBlobClient(this.GetFullBlobPath(blobName));

            await blobClient.UploadAsync(BinaryData.FromString(blobContent), true);
        }

        private BlobClient GetBlobClient(string blobName)
        {
            var containerClient = this.GetContainer();
            var blobClient = containerClient.GetBlobClient(this.GetFullBlobPath(blobName));
            return blobClient;
        }

        public IEnumerable<BlobItem> GetBlobsCustom(BlobContainerClient container, string prefix)
        {
            foreach (BlobItem blob in container.GetBlobs(BlobTraits.None, BlobStates.None, prefix))
            {
                yield return blob;
            }
        }

        private async Task CreateContainerAsync()
        {
            await GetContainer().CreateIfNotExistsAsync();
        }

        private async Task ClearContainerAsync()
        {
            var container = GetContainer();
            foreach (BlobItem blob in container.GetBlobs(BlobTraits.None, BlobStates.None))
            {
                await container.GetBlobClient(blob.Name).DeleteAsync();
            }
        }

        private BlobServiceClient GetBlobServiceClient() => new (CONNECTION_STRING);

        private BlobContainerClient GetContainer()
        {
            var blobServiceClient = this.GetBlobServiceClient();
            var containerClient = blobServiceClient.GetBlobContainerClient(BLOB_CONTAINER);

            return containerClient;
        }

        private string GetPathCombine(string path1, string path2)
        {
            return Path.Combine(path1, path2).Replace('\\', '/');
        }

        private static string RemoveFirstPathFromRelativeAddressName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrEmpty(ROOT_DIRECTORY))
            {
                return name;
            }

            var paths = name.Split(ROOT_DIRECTORY);
            name = paths[^1];
            if (name.StartsWith('/') && name.Length > 0)
            {
                return name[1..];
            }

            return name;
        }

        private string GetFileUri(string fileName)
        {
            var blobServiceClient = this.GetBlobServiceClient();
            var blobContainerUri = new Uri(blobServiceClient.Uri, BLOB_CONTAINER);
            var fileUri = new Uri($"{blobContainerUri}/{ROOT_DIRECTORY}/{fileName.ToLowerInvariant()}");

            return fileUri.ToString();
        }

        private string GetSignedFileUri(string fileName)
        {
            var blobServiceClient = this.GetBlobServiceClient();
            var containerClient = blobServiceClient.GetBlobContainerClient(BLOB_CONTAINER);
            var blobClient = containerClient.GetBlobClient(this.GetFullBlobPath(fileName));
            var sasUri = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddHours(1));

            return sasUri.ToString();
        }
    }
}


