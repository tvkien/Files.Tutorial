using Files.Tutorial.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Files.Tutorial.Handlers
{
    public interface IFileDatabaseService
    {
        Task UploadToDatabaseAsync(List<IFormFile> files, string description);

        Task<FileDownloadModel> DownloadFileFromDatabase(int id);

        Task DeleteFileFromDatabase(int id);
    }
}
