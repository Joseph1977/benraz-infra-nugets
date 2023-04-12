using FluentAssertions;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Benraz.Infrastructure.Files.Local;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Files.Tests
{
    [TestFixture]
    [Ignore("LocalFilesService integration tests.")]
    public class LocalFilesServiceTests
    {
        private const string DIRECTORY = "Files";
        private const string SUBDIRECTORY = "Folder001";
        private const string FILE1_NAME = "testfile001";
        private const string FILE2_NAME = "Folder001\\testfile002";

        private LocalFilesService _localFilesService;

        [SetUp]
        public Task SetUpAsync()
        {
            CreateDirectory();
            ClearDirectory();

            var settings = new LocalFilesServiceSettings
            {
                Directory = "Files",
                BaseUrl = "http://localhost/",
            };
            _localFilesService = new LocalFilesService(Options.Create(settings));

            return Task.CompletedTask;
        }

        [TearDown]
        public Task TearDownAsync()
        {
            ClearDirectory();
            return Task.CompletedTask;
        }

        [Test]
        public async Task FindAllAsync_OnlyMetadata_ReturnsFilesMetadata()
        {
            await UploadFileAsync(FILE1_NAME, "Content001");

            var files = await _localFilesService.FindAllAsync(null, FileProperties.Metadata);

            files.Should().NotBeNullOrEmpty();
            files[0].Name.Should().Be(FILE1_NAME);
            files[0].Content.Should().BeNull();
        }

        [Test]
        public async Task FindAllAsync_AllProperties_ReturnsFiles()
        {
            await UploadFileAsync(FILE1_NAME, "Content001");

            var files = await _localFilesService.FindAllAsync(null, FileProperties.All);

            files.Should().NotBeNullOrEmpty();
            files[0].Name.Should().Be(FILE1_NAME);
            files[0].Content.Should().Match(x => Encoding.UTF8.GetString(x.ToArray()) == "Content001");
        }

        [Test]
        public async Task FindAllAsync_DirectoryDoesntExist_ReturnsNoFiles()
        {
            var files = await _localFilesService.FindAllAsync("WrongFolder", FileProperties.Metadata);
            files.Should().BeEmpty();
        }

        [Test]
        public async Task FindAllAsync_WithPath_ReturnsFilesMetadata()
        {
            await UploadFileAsync(FILE1_NAME, "Content001");
            await UploadFileAsync(FILE2_NAME, "Content002");

            var files = await _localFilesService.FindAllAsync(SUBDIRECTORY, FileProperties.Metadata);

            files.Should().NotBeNullOrEmpty();
            files[0].Name.Should().Be(FILE2_NAME);
            files[0].Uri.Should().Be("http://localhost/Folder001/testfile002");
        }

        [Test]
        public async Task FindByNameAsync_TopLevelAllPropertiesFileExist_ReturnsFile()
        {
            await UploadFileAsync(FILE1_NAME, "Content001");

            var file = await _localFilesService.FindByNameAsync(FILE1_NAME, FileProperties.All);

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

            await _localFilesService.CreateAsync(file);

            file.Uri.Should().Be("http://localhost/Folder001/testfile002");

            var retrievedFile = GetFile(FILE2_NAME);
            retrievedFile.Exists.Should().BeTrue();
        }

        [Test]
        public async Task CreateFolderAsync_NewFolder_CreatesFolder()
        {
            var folderName = "NewFolder";
            var path = Path.Combine(Directory.GetCurrentDirectory(), DIRECTORY);
            var status = await _localFilesService.CreateFolderAsync(folderName, path);

            status.Should().BeTrue();

            await _localFilesService.DeletePathAsync(Path.Combine(path, folderName));
        }

        [Test]
        public async Task MoveFileAsync_FileAndSourceAndDestination_RemoveFileFromSourceAndCreateOnDestination()
        {
            var sourceFolderName = "sourceFolder";
            var destinationFolderName = "destinationFolder";
            var sourcePath = Path.Combine(Directory.GetCurrentDirectory(), DIRECTORY);
            var destinationPath = Path.Combine(Directory.GetCurrentDirectory(), DIRECTORY);
            await _localFilesService.CreateFolderAsync(sourceFolderName, sourcePath);
            await _localFilesService.CreateFolderAsync(destinationFolderName, destinationPath);
            sourcePath = Path.Combine(sourcePath, sourceFolderName);
            destinationPath = Path.Combine(destinationPath, destinationFolderName);
            await UploadFileAsync(sourcePath, FILE1_NAME, "Content001");
            var status = await _localFilesService.MoveFileAsync(FILE1_NAME, sourcePath, destinationPath);

            status.Should().BeTrue();

            await _localFilesService.DeletePathAsync(sourcePath);
            await _localFilesService.DeletePathAsync(destinationPath);
        }

        [Test]
        public async Task RenameAsync_FileExists_MovesFile()
        {
            await UploadFileAsync(FILE1_NAME, "Content001");

            var file = new File
            {
                Name = FILE1_NAME
            };

            await _localFilesService.RenameAsync(file, FILE2_NAME);

            file.Uri.Should().Be("http://localhost/Folder001/testfile002");

            GetFile(FILE1_NAME).Exists.Should().BeFalse();
            GetFile(FILE2_NAME).Exists.Should().BeTrue();
        }

        [Test]
        public async Task DeleteAsync_FileExists_DeletesFile()
        {
            await UploadFileAsync(FILE1_NAME, "Content001");

            await _localFilesService.DeleteAsync(FILE1_NAME);

            GetFile(FILE1_NAME).Exists.Should().BeFalse();
        }

        [Test]
        public async Task DeleteAllAsync_FromRoot_DeletesFiles()
        {
            await UploadFileAsync(FILE1_NAME, "Content001");

            await _localFilesService.DeleteAllAsync(null);

            GetFile(FILE1_NAME).Exists.Should().BeFalse();
        }

        [Test]
        public async Task DeletePathAsync_PathExists_DeletesPath()
        {
            await UploadFileAsync(FILE2_NAME, "Content002");

            await _localFilesService.DeletePathAsync(SUBDIRECTORY);

            var subdirectoryFullName = Path.Combine(GetDirectory().FullName, SUBDIRECTORY);
            new DirectoryInfo(subdirectoryFullName).Exists.Should().BeFalse();
        }

        [Test]
        public void GetUri_CorrectSettings_ReturnsUri()
        {
            var uri = _localFilesService.GetUri(FILE1_NAME);
            uri.Should().Be("http://localhost/testfile001");
        }

        private async Task UploadFileAsync(string name, string content)
        {
            var fileName = Path.Combine(GetDirectory().FullName, name);
            var file = new FileInfo(fileName);

            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }

            using (var writer = file.AppendText())
            {
                await writer.WriteAsync(content);
            }
        }

        private async Task UploadFileAsync(string path, string name, string content)
        {
            var fileName = Path.Combine(path, name);
            var file = new FileInfo(fileName);

            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }

            using (var writer = file.AppendText())
            {
                await writer.WriteAsync(content);
            }
        }

        private FileInfo GetFile(string name)
        {
            return new FileInfo(Path.Combine(GetDirectory().FullName, name));
        }

        private void CreateDirectory()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), DIRECTORY);
            var directory = new DirectoryInfo(path);
            if (!directory.Exists)
            {
                directory.Create();
            }
        }

        private void ClearDirectory()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), DIRECTORY);

            var directoryNames = Directory.GetDirectories(path);
            foreach (var directoryName in directoryNames)
            {
                Directory.Delete(directoryName, true);
            }

            var fileNames = Directory.GetFiles(path);
            foreach (var fileName in fileNames)
            {
                System.IO.File.Delete(fileName);
            }
        }

        private DirectoryInfo GetDirectory()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), DIRECTORY);
            var directory = new DirectoryInfo(path);

            return directory;
        }
    }
}


