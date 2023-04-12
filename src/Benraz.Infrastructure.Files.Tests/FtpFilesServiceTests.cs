using FluentAssertions;
using FluentFTP;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Benraz.Infrastructure.Files.FTP;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Files.Tests
{
    [TestFixture]
    [Ignore("FtpService integration tests.")]
    [NonParallelizable]
    public class FtpFilesServiceTests
    {
        private const string BASE_URL = "ftp://localhost:21";
        private const string USER_NAME = "anonymous";
        private const string PASSWORD = "anonymous";
        private const string ROOT_DIRECTORY = "";
        private const string SUBDIRECTORY = "folder001";
        private const string FILE1_NAME = "testblob001";
        private const string FILE2_NAME = "folder001/testblob002";


        private FtpFilesService _ftpFilesService;

        [OneTimeSetUp]
        public async Task SetUpAsync()
        {
            await Cleanup();
            await CreateDirectory();
            var settings = new FtpFilesServiceSettings
            {
                BaseUrl = BASE_URL,
                UserName = USER_NAME,
                Password = PASSWORD,
                RootDirectory = ROOT_DIRECTORY
            };
            _ftpFilesService = new FtpFilesService(Options.Create(settings));
        }

        [OneTimeTearDown]
        public async Task TearDownAsync()
        {
            await Cleanup();
        }

        [Test]
        public async Task FindAllAsync_OnlyMetadata_ReturnsFilesMetadata()
        {
            await UploadFileAsync(FILE1_NAME, "Content001");

            var files = await _ftpFilesService.FindAllAsync(null, FileProperties.Metadata);

            files.Should().NotBeNullOrEmpty();
            files[0].Name.Should().Be(FILE1_NAME);
            files[0].Content.Should().BeNull();
        }

        [Test]
        public async Task FindAllAsync_AllProperties_ReturnsFiles()
        {
            string fileName = FILE1_NAME + "998";
            await UploadFileAsync(fileName, "Content001");

            var files = await _ftpFilesService.FindAllAsync(null, FileProperties.All);

            files.Should().NotBeNullOrEmpty();
            files[0].Name.Should().Be(fileName);
            files[0].Content.Should().Match(x => Encoding.UTF8.GetString(x.ToArray()) == "Content001");
        }

        [Test]
        public async Task FindAllAsync_DirectoryDoesntExist_ReturnsNoFiles()
        {
            var files = await _ftpFilesService.FindAllAsync("WrongFolder", FileProperties.Metadata);
            files.Should().BeEmpty();
        }

        [Test]
        public async Task FindAllAsync_WithPath_ReturnsFilesMetadata()
        {
            await UploadFileAsync(FILE1_NAME, "Content001");
            await UploadFileAsync(FILE2_NAME, "Content002");

            var files = await _ftpFilesService.FindAllAsync(SUBDIRECTORY, FileProperties.Metadata);

            files.Should().NotBeNullOrEmpty();
            files[0].Name.Should().Be(GetFileNameFromRelativePath(FILE2_NAME));
            files[0].Uri.Should().Be(GetFileUri(FILE2_NAME));
        }

        [Test]
        public async Task FindByNameAsync_TopLevelAllPropertiesFileExist_ReturnsFile()
        {
            string fileName = FILE1_NAME + "999";
            await UploadFileAsync(fileName, "Content001");

            var file = await _ftpFilesService.FindByNameAsync(fileName, FileProperties.All);

            file.Should().NotBeNull();
            file.Content.Should().Match(x => Encoding.UTF8.GetString(x.ToArray()) == "Content001");
        }

        [Test]
        public async Task CreateAsync_NewFile_CreatesFile()
        {
            var file = new File
            {
                Name = FILE2_NAME,
                Content = Encoding.UTF8.GetBytes("Content001")
            };

            await _ftpFilesService.CreateAsync(file);

            file.Uri.Should().Be(GetFileUri(FILE2_NAME));

            var retrievedFile = await _ftpFilesService.FindByNameAsync(FILE2_NAME, FileProperties.All);
            retrievedFile.Should().NotBeNull();
            retrievedFile.Content.Should().Match(x => Encoding.UTF8.GetString(x.ToArray()) == "Content001");
            retrievedFile.Name.Should().Be(GetFileNameFromRelativePath(FILE2_NAME));
        }

        [Test]
        public async Task CreateFolderAsync_NewFolder_CreatesFolder()
        {
            var folderName = "NewFolder";
            var status = await _ftpFilesService.CreateFolderAsync(folderName, SUBDIRECTORY);

            status.Should().BeTrue();

            await _ftpFilesService.DeletePathAsync(Path.Combine(SUBDIRECTORY, folderName));
        }

        [Test]
        public async Task MoveFileAsync_FileAndSourceAndDestination_RemoveFileFromSourceAndCreateOnDestination()
        {
            var sourceFolderName = "sourceFolder";
            var destinationFolderName = "destinationFolder";
            await _ftpFilesService.CreateFolderAsync(sourceFolderName, SUBDIRECTORY);
            await _ftpFilesService.CreateFolderAsync(destinationFolderName, SUBDIRECTORY);
            var sourcePath = Path.Combine(SUBDIRECTORY, sourceFolderName);
            var destinationPath = Path.Combine(SUBDIRECTORY, destinationFolderName);
            await UploadFileAsync(Path.Combine(sourcePath, FILE1_NAME), "Content001");
            var status = await _ftpFilesService.MoveFileAsync(FILE1_NAME, sourcePath, destinationPath);

            status.Should().BeTrue();

            await _ftpFilesService.DeletePathAsync(sourcePath);
            await _ftpFilesService.DeletePathAsync(destinationPath);
        }

        [Test]
        public async Task RenameAsync_FileExists_MovesFile()
        {
            await UploadFileAsync(FILE1_NAME, "Content001");

            var file = new File
            {
                Name = FILE1_NAME
            };

            await _ftpFilesService.RenameAsync(file, FILE2_NAME);

            file.Uri.Should().Be(GetFileUri(FILE2_NAME));

            (await GetFile(FILE1_NAME)).Should().BeNull();
            (await GetFile(FILE2_NAME)).Should().NotBeNull();
        }

        [Test]
        public async Task DeleteAsync_FileExists_DeletesFile()
        {
            await UploadFileAsync(FILE1_NAME, "Content001");

            await _ftpFilesService.DeleteAsync(FILE1_NAME);

            (await GetFile(FILE1_NAME)).Should().BeNull();
        }

        [Test]
        public async Task DeleteAllAsync_FromRoot_DeletesFiles()
        {
            await UploadFileAsync(FILE1_NAME, "Content001");

            await _ftpFilesService.DeleteAllAsync(null);

            (await GetFile(FILE1_NAME)).Should().BeNull();
        }

        [Test]
        public async Task DeletePathAsync_PathExists_DeletesPath()
        {
            await UploadFileAsync(FILE2_NAME, "Content002");

            await _ftpFilesService.DeletePathAsync(SUBDIRECTORY);

            (await GetFile(FILE2_NAME)).Should().BeNull();
        }

        [Test]
        public void GetUri_CorrectSettings_ReturnsUri()
        {
            var uri = _ftpFilesService.GetUri(FILE1_NAME);
            uri.Should().Be(GetFileUri(FILE1_NAME));
        }

        private async Task UploadFileAsync(string name, string content)
        {
            File file = new File()
            {
                Content = Encoding.UTF8.GetBytes(content),
                CreateTimeUtc = DateTime.UtcNow,
                UpdateTimeUtc = DateTime.UtcNow,
                Uri = GetRelativeRootFilePath(name),
                Name = name
            };
            await _ftpFilesService.CreateAsync(file);
        }

        private async Task<File> GetFile(string fileName)
        {
            return await _ftpFilesService.FindByNameAsync(fileName, FileProperties.Metadata);
        }

        private async Task CreateDirectory()
        {
            await GetFtpConnection();
        }

        private async Task Cleanup()
        {
            var (client, token) = await GetFtpConnection();
            using (client)
            {
                if (!string.IsNullOrEmpty(ROOT_DIRECTORY))
                {
                    if (await client.DirectoryExistsAsync($"/{ROOT_DIRECTORY}",token))
                        await client.DeleteDirectoryAsync($"/{ROOT_DIRECTORY}",token);
                }
                else
                {
                    await client.DeleteDirectoryAsync($"/", FtpListOption.Recursive,token);
                }
            }
        }

        private async Task<(FtpClient client, CancellationToken token)> GetFtpConnection()
        {
            var token = new CancellationToken();
            FtpClient client = new FtpClient(new Uri(BASE_URL), USER_NAME, PASSWORD);
            client.EncryptionMode = FtpEncryptionMode.Explicit;
            client.ValidateAnyCertificate = true;
            await client.ConnectAsync(token);
            if (!string.IsNullOrEmpty(ROOT_DIRECTORY))
            {
                if (!await client.DirectoryExistsAsync($"/{ROOT_DIRECTORY}", token))
                    await client.CreateDirectoryAsync($"/{ROOT_DIRECTORY}", token);
                await client.SetWorkingDirectoryAsync($"/{ROOT_DIRECTORY}", token);
            }

            return (client, token);
        }

        private string GetRelativeRootFilePath(string fileName, string directory = ROOT_DIRECTORY)
        {
            return Path.Combine(directory, fileName);
        }

        private string GetFileNameFromRelativePath(string relativePath)
        {
            return relativePath.Split('/')[^1];
        }

        private string GetFileUri(string fileNamePath)
        {
            var rootDirectory = ROOT_DIRECTORY;
            if (!fileNamePath.StartsWith(rootDirectory))
            {
                fileNamePath = Path.Combine(rootDirectory, fileNamePath);
            }
            return new Uri(new Uri(BASE_URL), fileNamePath).ToString();
        }
    }
}


