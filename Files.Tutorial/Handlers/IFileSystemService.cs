using Files.Tutorial.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Files.Tutorial.Handlers
{
    public interface IFileSystemService
    {
        Task<FileUploadViewModel> LoadAllFilesAsync();

        Task UploadToFileSystemAsync(List<IFormFile> files, string description);

        Task<FileDownloadModel> DownloadFileFromFileSystem(int id);

        Task DeleteFileFromFileSystem(int id);
    }
}