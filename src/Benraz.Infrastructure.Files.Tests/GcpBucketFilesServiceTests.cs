using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Benraz.Infrastructure.Files.GCP;
using FluentAssertions;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace Benraz.Infrastructure.Files.Tests;

[TestFixture]
[Ignore("Gcp Bucket files service integration tests.")]
public class GcpBucketFilesServiceTests
{
    private const string FOLDER = "root001/folder001";
    private const string FILE1_NAME = "testfile001";
    private const string FILE2_NAME = "folder001/testfile002";
    private const string ROOT_DIRECTORY = "root001";
    private const string BUCKET_NAME = "strgbucket-dev-usc1poc-ai";
    private const string SA_JSON = @"";

    private readonly GoogleCredential CREDENTIALS;
    
    private GcpBucketFilesService _gcpBucketFilesService;
    
    [SetUp]
    public async Task SetUpAsync()
    {
        var settings = new GcpBucketFilesServiceSettings()
        {
            BucketName = BUCKET_NAME,
            GcpApplicationCredentials = SA_JSON
        };
        _gcpBucketFilesService = new GcpBucketFilesService(Options.Create(settings));

        //await ClearFolderAsync();
    }
    
    [TearDown]
    public async Task TearDownAsync()
    {
       await ClearFolderAsync();
    }
    
    [Test]
    public async Task FindAllAsync_OnlyMetadata_ReturnsFilesMetadata()
    {
        await UploadFileAsync(FILE1_NAME, "TestFileContent001");
        
        var files = await _gcpBucketFilesService.FindAllAsync(null, FileProperties.Metadata);

        files.Should().NotBeNullOrEmpty();
        RemoveFirstPathFromRelativeAddressName(files[0].Name).Should().Be(FILE1_NAME);
        files[0].Content.Should().BeNull();
    }
    
    [Test]
    public async Task FindAllAsync_AllProperties_ReturnsFiles()
    {
        await UploadFileAsync(FILE1_NAME, "TestFileContent001");

        var files = await _gcpBucketFilesService.FindAllAsync(null, FileProperties.All);

        files.Should().NotBeNullOrEmpty();
        RemoveFirstPathFromRelativeAddressName(files[0].Name).Should().Be(FILE1_NAME);
        files[0].Content.Should().Match(x => Encoding.UTF8.GetString(x.ToArray()) == "TestFileContent001");
    }

    [Test]
    public async Task FindAllAsync_WithPath_ReturnsFilesMetadata()
    {
        await UploadFileAsync(FILE1_NAME, "TestFileContent001");
        await UploadFileAsync(FILE2_NAME, "TestFileContent002");

        var files = await _gcpBucketFilesService.FindAllAsync(FOLDER, FileProperties.Metadata);

        files.Should().NotBeNullOrEmpty();
        RemoveFirstPathFromRelativeAddressName(files[0].Name).Should().Be(FILE2_NAME);
    }
    
    [Test]
    public async Task FindByNameAsync_ContainerAndBlobsExist_ReturnsFiles()
    {
        await UploadFileAsync(FILE1_NAME, "TestFileContent001");

        var file = await _gcpBucketFilesService.FindByNameAsync(ROOT_DIRECTORY + "/" + FILE1_NAME, FileProperties.All);

        file.Should().NotBeNull();
        file.Content.Should().Match(x => Encoding.UTF8.GetString(x.ToArray()) == "TestFileContent001");
    }
    
    [Test]
    public async Task CreateAsync_ContainerAndBlobsExist_ReturnsFiles()
    {
        await UploadFileAsync(FILE1_NAME, "TestFileContent001");

        var file = await _gcpBucketFilesService.FindByNameAsync(ROOT_DIRECTORY + "/" + FILE1_NAME, FileProperties.All);

        file.Should().NotBeNull();
        file.Content.Should().Match(x => Encoding.UTF8.GetString(x.ToArray()) == "TestFileContent001");
    }
    
    [Test]
    public async Task RenameAsync_ContainerAndBlobExist_MovesFile()
    {
        await UploadFileAsync(FILE1_NAME, "TestFileContent001");

        var file = new File
        {
            Name = ROOT_DIRECTORY + "/" + FILE1_NAME
        };
        await _gcpBucketFilesService.RenameAsync(file, ROOT_DIRECTORY + "/" + FILE2_NAME);

        var file1 = await _gcpBucketFilesService.FindByNameAsync(ROOT_DIRECTORY + "/" + FILE1_NAME, FileProperties.All);
        var file2 = await _gcpBucketFilesService.FindByNameAsync(ROOT_DIRECTORY + "/" + FILE2_NAME, FileProperties.All);
        
        file1.Should().BeNull();
        file2.Should().NotBeNull();
    }
    
    [Test]
    public async Task DeleteAsync_ContainerAndBlobsExist_DeletesFile()
    {
        await UploadFileAsync(FILE1_NAME, "TestFileContent001");

        await _gcpBucketFilesService.DeleteAsync(ROOT_DIRECTORY + "/" + FILE1_NAME);

        var file = await _gcpBucketFilesService.FindByNameAsync(ROOT_DIRECTORY + "/" + FILE1_NAME, FileProperties.All);
        file.Should().BeNull();
    }
    
    [Test]
    public async Task DeleteAllAsync_FromRoot_DeletesFiles()
    {
        await UploadFileAsync(FILE1_NAME, "TestFileContent001");

        await _gcpBucketFilesService.DeleteAllAsync(ROOT_DIRECTORY);

        var file = await _gcpBucketFilesService.FindByNameAsync(ROOT_DIRECTORY + "/" + FILE1_NAME, FileProperties.All);
        file.Should().BeNull();
    }
    
    [Test]
    public async Task DeletePathAsync_PathExists_DeletesPath()
    {
        await UploadFileAsync(FILE2_NAME, "TestFileContent002");

        await _gcpBucketFilesService.DeletePathAsync(FOLDER);

        var file = await _gcpBucketFilesService.FindByNameAsync(ROOT_DIRECTORY + "/" + FILE2_NAME, FileProperties.All);
        file.Should().BeNull();
    }
    
    [Test]
    public async Task GetUri_CorrectSettings_ReturnsUri()
    {
        await UploadFileAsync(FILE1_NAME, "TestFileContent001");

        var uri = _gcpBucketFilesService.GetUri(ROOT_DIRECTORY + "/" + FILE1_NAME);

        uri.Should().Be($"https://storage.googleapis.com/{BUCKET_NAME}/{Uri.EscapeDataString(ROOT_DIRECTORY + "/" + FILE1_NAME)}");
    }
    
    [Test]
    public async Task GetSignedUri_CorrectSettings_ReturnsUri()
    {
        await UploadFileAsync(FILE1_NAME, "TestFileContent001");

        var uri = _gcpBucketFilesService.GetSignedUri(ROOT_DIRECTORY + "/" + FILE1_NAME);

        uri.Should().Be(GetSignedUri(ROOT_DIRECTORY + "/" + FILE1_NAME));
    }
    
    [Test]
    public async Task MoveFileAsync_FileAndSourceAndDestination_RemoveFileFromSourceAndCreateOnDestination()
    {
        var sourceFolderName = "sourcefolder";
        var destinationFolderName = "destinationfolder";
        var fileName = "test.txt";
        var sourcePath = GetPathCombine(FOLDER, sourceFolderName);
        var destinationPath = GetPathCombine(FOLDER, destinationFolderName);
        var filePath = GetPathCombine(sourcePath, fileName);

        await UploadFileAsync(filePath, "TestFileContent001", false);
        
        var status = await _gcpBucketFilesService.MoveFileAsync(fileName, sourcePath, destinationPath);

        status.Should().BeTrue();

        await _gcpBucketFilesService.DeletePathAsync(destinationPath);
        await _gcpBucketFilesService.DeletePathAsync(sourcePath);
        await _gcpBucketFilesService.DeletePathAsync(FOLDER);
    }

    private async Task ClearFolderAsync()
    {
        await _gcpBucketFilesService.DeletePathAsync(ROOT_DIRECTORY);
    }
    
    private async Task UploadFileAsync(string fileName, string fileContent, bool withRootDirectory = true)
    {
        var file = new File()
        {
            Name = (withRootDirectory ? ROOT_DIRECTORY + "/" : "") + fileName,
            Content = Encoding.UTF8.GetBytes(fileContent)
        };
        
        await _gcpBucketFilesService.CreateAsync(file);
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
    
    public string GetSignedUri(string fileName, int expiresInHours = 1)
    {
        var urlSigner = UrlSigner.FromCredential(CREDENTIALS);
        var url = urlSigner.Sign(
            bucket: BUCKET_NAME,
            objectName: fileName,
            duration: TimeSpan.FromHours(expiresInHours),
            httpMethod: HttpMethod.Get
        );

        return url;
    }
    
    private string GetPathCombine(string path1, string path2)
    {
        return Path.Combine(path1, path2).Replace('\\', '/');
    }
}