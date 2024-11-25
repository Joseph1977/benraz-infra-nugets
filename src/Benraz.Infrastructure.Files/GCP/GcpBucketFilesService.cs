using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.Control.V2;
using Google.Cloud.Storage.V1;
using Grpc.Core;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;

namespace Benraz.Infrastructure.Files.GCP;

/// <summary>
/// Google Cloud Platform Bucket files service.
/// </summary>
public class GcpBucketFilesService : IFilesService
{
    private readonly GcpBucketFilesServiceSettings _settings;
    private readonly StorageClient _storageClient;
    private readonly StorageControlClient _storageControl;
    private readonly GoogleCredential _credential;
    
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="settings">Settings.</param>
    public GcpBucketFilesService(IOptions<GcpBucketFilesServiceSettings> settings)
    {
        _settings = settings.Value;

        _credential = string.IsNullOrEmpty(_settings.GcpApplicationCredentials) ? GoogleCredential.GetApplicationDefault() : GoogleCredential.FromJson(_settings.GcpApplicationCredentials);
        _storageClient = StorageClient.Create(_credential);
        _storageControl = StorageControlClient.Create();
    }
    
    /// <summary>
    /// Returns all files from location.
    /// </summary>
    /// <param name="path">Path to files.</param>
    /// <param name="fileProperties">File properties to retrieve.</param>
    /// <returns>Files.</returns>
    public async Task<File[]> FindAllAsync(string path, FileProperties fileProperties)
    {
        var files = new List<File>();
        var objects = _storageClient.ListObjectsAsync(_settings.BucketName, path);
        
        await foreach (var obj in objects)
        {
            var file = new File();

            if (fileProperties.HasFlag(FileProperties.Metadata) || fileProperties.HasFlag(FileProperties.All))
            {
                file.Name = obj.Name;
                file.Uri = GetUri(obj.Name);
                file.CreateTimeUtc = obj.TimeCreated?.ToUniversalTime();
                file.UpdateTimeUtc = obj.Updated?.ToUniversalTime();
            }
            
            if (fileProperties.HasFlag(FileProperties.Content) || fileProperties.HasFlag(FileProperties.All))
            {
                using var memoryStream = new MemoryStream();
                await _storageClient.DownloadObjectAsync(obj, memoryStream);
                file.Content = memoryStream.ToArray();
            }

            files.Add(file);
        }

        return files.ToArray();
    }

    /// <summary>
    /// Returns file with specified name from location.
    /// </summary>
    /// <param name="fileName">File name.</param>
    /// <param name="fileProperties">File properties to retrieve.</param>
    /// <returns>File.</returns>
    public async Task<File> FindByNameAsync(string fileName, FileProperties fileProperties)
    {
        try
        {
            var obj = await _storageClient.GetObjectAsync(_settings.BucketName, fileName);

            var file = new File();

            if (fileProperties.HasFlag(FileProperties.Metadata) || fileProperties.HasFlag(FileProperties.All))
            {
                file.Name = obj.Name;
                file.Uri = GetUri(obj.Name);
                file.CreateTimeUtc = obj.TimeCreated?.ToUniversalTime();
                file.UpdateTimeUtc = obj.Updated?.ToUniversalTime();
            }

            if (fileProperties.HasFlag(FileProperties.Content) || fileProperties.HasFlag(FileProperties.All))
            {
                using var memoryStream = new MemoryStream();
                await _storageClient.DownloadObjectAsync(obj, memoryStream);
                file.Content = memoryStream.ToArray();
            }

            return file;
        }
        catch (GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

    }

    /// <summary>
    /// Creates BLOB in container and returns file with URI updated.
    /// </summary>
    /// <param name="file">File.</param>
    /// <returns>File.</returns>
    public async Task<File> CreateAsync(File file)
    {
        using var stream = new MemoryStream(file.Content);
        var obj = await _storageClient.UploadObjectAsync(
            bucket: _settings.BucketName,
            objectName: file.Name,
            contentType: GetContentType(file.Name),
            source: stream
        );

        file.Uri = GetUri(file.Name);
        file.CreateTimeUtc = obj.TimeCreated?.ToUniversalTime();
        file.UpdateTimeUtc = obj.Updated?.ToUniversalTime();

        return file;
    }

    /// <summary>
    /// Renames file and returns file with name and URL updated.
    /// </summary>
    /// <param name="file">File to rename.</param>
    /// <param name="newFileName">New file name.</param>
    /// <returns>Updated file.</returns>
    public async Task<File> RenameAsync(File file, string newFileName)
    {
        var destinationObject = await _storageClient.CopyObjectAsync(
            _settings.BucketName, file.Name, _settings.BucketName, newFileName
        );

        await _storageClient.DeleteObjectAsync(_settings.BucketName, file.Name);

        file.Name = newFileName;
        file.Uri = GetUri(newFileName);
        file.UpdateTimeUtc = destinationObject.Updated?.ToUniversalTime();

        return file;
    }

    /// <summary>
    /// Deletes file with specified name from container.
    /// </summary>
    /// <param name="fileName">File name.</param>
    /// <returns>Task.</returns>
    public async Task DeleteAsync(string fileName)
    {
        await _storageClient.DeleteObjectAsync(_settings.BucketName, fileName);
    }

    /// <summary>
    /// Deletes all files and folders in a particular path.
    /// </summary>
    /// <param name="path">Path to files.</param>
    /// <returns>Task.</returns>
    public async Task DeleteAllAsync(string path)
    {
        string folderPath = path.EndsWith("/") ? path : path + "/";

        var allObjects = _storageClient.ListObjectsAsync(_settings.BucketName, folderPath);

        var deleteTasks = new List<Task>();
        var folderPlaceholders = new HashSet<string>();
        
        await foreach (var obj in allObjects)
        {
            deleteTasks.Add(_storageClient.DeleteObjectAsync(obj));
        
            string objectName = obj.Name;
            while (objectName.Contains('/'))
            {
                objectName = objectName.Substring(0, objectName.LastIndexOf('/'));
                string folderPlaceholder = objectName + "/";
                folderPlaceholders.Add(folderPlaceholder);
            }
        
            if (deleteTasks.Count >= 100)
            {
                await Task.WhenAll(deleteTasks);
                deleteTasks.Clear();
            }
        }
        
        if (deleteTasks.Count > 0)
        {
            await Task.WhenAll(deleteTasks);
        }
        
        foreach (var folderPlaceholder in folderPlaceholders)
        {
            if (folderPlaceholder == folderPath || folderPath.Length > folderPlaceholder.Length)
            {
                continue;
            }
            
            try
            {
                await _storageControl.DeleteFolderAsync(GetFoldersResource(folderPlaceholder));
            }
            catch (RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
            {
            }
        }
    }

    /// <summary>
    /// Deletes a folder and all its contents.
    /// </summary>
    /// <param name="path">Path to the folder.</param>
    /// <returns>Task.</returns>
    public async Task DeletePathAsync(string path)
    {
        string folderPath = path.EndsWith("/") ? path : path + "/";
        
        await DeleteAllAsync(folderPath);

        try
        {
            await _storageControl.DeleteFolderAsync(GetFoldersResource(folderPath));
        }
        catch (RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
        }
    }

    /// <summary>
    /// Returns file URI.
    /// </summary>
    /// <param name="fileName">File name.</param>
    /// <returns>File URI.</returns>
    public string GetUri(string fileName)
    {
        return $"https://storage.googleapis.com/{_settings.BucketName}/{Uri.EscapeDataString(fileName)}";
    }

    /// <summary>
    /// Move file from source to destination.
    /// </summary>
    /// <param name="fileName">File name.</param>
    /// <param name="sourcePath">Source path.</param>
    /// <param name="destinationPath">Destination path.</param>
    /// <returns>Transfer status.</returns>
    public async Task<bool> MoveFileAsync(string fileName, string sourcePath, string destinationPath)
    {
        var sourceObjectName = $"{sourcePath}/{fileName}".Trim('/');
        var destinationObjectName = $"{destinationPath}/{fileName}".Trim('/');
        await _storageClient.CopyObjectAsync(_settings.BucketName, sourceObjectName, _settings.BucketName, destinationObjectName);
        await _storageClient.DeleteObjectAsync(_settings.BucketName, sourceObjectName);

        return true;
    }

    /// <summary>
    /// Create new folder by path.
    /// </summary>
    /// <param name="folderName">Folder name.</param>
    /// <param name="path">Path.</param>
    /// <returns>Creation status.</returns>
    public async Task<bool> CreateFolderAsync(string folderName, string path)
    {
        var folderObjectName = $"{path}/{folderName}/".Trim('/');

        StorageControlClient storageControl = StorageControlClient.Create();

        var request = new CreateFolderRequest
        {
            Parent = BucketName.FormatProjectBucket("_", _settings.BucketName),
            FolderId = folderObjectName
        };

        await storageControl.CreateFolderAsync(request);

        return true;
    }

    /// <summary>
    /// Returns signed file URI.
    /// </summary>
    /// <param name="fileName">File name.</param>
    /// <param name="expiresInHours">Expires in hours.</param>
    /// <returns>Signed file URI.</returns>
    public string GetSignedUri(string fileName, int expiresInHours = 1)
    {
        var urlSigner = UrlSigner.FromCredential(_credential);
        var url = urlSigner.Sign(
            bucket: _settings.BucketName,
            objectName: fileName,
            duration: TimeSpan.FromHours(expiresInHours),
            httpMethod: HttpMethod.Get
        );

        return url;
    }

    private string GetContentType(string fileName)
    {
        if (!new FileExtensionContentTypeProvider().TryGetContentType(fileName, out var contentType))
        {
            contentType = "application/octet-stream";
        }

        if (contentType == "text/html")
        {
            contentType = "text/html; charset=utf-8";
        }

        return contentType;
    }

    private string GetFoldersResource(string path)
    {
        return FolderName.FormatProjectBucketFolder("_", _settings.BucketName, path.Trim(('/')));
    }
}