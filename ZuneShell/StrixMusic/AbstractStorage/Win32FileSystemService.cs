#if OPENZUNE

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace OwlCore.AbstractStorage;

public class Win32FileSystemService : IFileSystemService
{
    public Win32FileSystemService(string rootFolder)
    {
        RootFolder = new SystemIOFolderData(rootFolder);
    }

    public IFolderData RootFolder { get; }

    public bool IsInitialized => true;

    public Task<IFolderData> CreateDirectoryAsync(string folderName)
        => RootFolder.CreateFolderAsync(folderName);

    public Task<bool> DirectoryExistsAsync(string path) => Task.FromResult(Directory.Exists(path));

    public Task<bool> FileExistsAsync(string path) => Task.FromResult(File.Exists(path));

    public Task<IFileData> GetFileFromPathAsync(string path)
        => Task.FromResult<IFileData>(new SystemIOFileData(path));

    public Task<IFolderData> GetFolderFromPathAsync(string path)
        => Task.FromResult<IFolderData>(new SystemIOFolderData(path));

    public Task<IReadOnlyList<IFolderData>> GetPickedFolders()
    {
        throw new System.NotImplementedException();
    }

    public Task InitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    public Task<IFolderData> PickFolder()
    {
        return Task.FromResult(RootFolder);
    }

    public Task RevokeAccess(IFolderData folder) => Task.CompletedTask;
}

#endif