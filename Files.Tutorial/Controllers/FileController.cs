using Files.Tutorial.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Files.Tutorial.Controllers
{

    [DisableRequestSizeLimit]
    public class FileController : Controller
    {
        private readonly IFileSystemService fileSystemService;
        private readonly IFileDatabaseService fileDatabaseService;

        public FileController(IFileSystemService fileSystemService, IFileDatabaseService fileDatabaseService)
        {
            this.fileSystemService = fileSystemService;
            this.fileDatabaseService = fileDatabaseService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var fileuploadViewModel = await fileSystemService.LoadAllFilesAsync();
            return View(fileuploadViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UploadToFileSystem(List<IFormFile> files, string description)
        {
            await fileSystemService.UploadToFileSystemAsync(files, description);
            TempData["Message"] = "File successfully uploaded to File System.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DownloadFileFromFileSystem(int id)
        {
            var response = await fileSystemService.DownloadFileFromFileSystem(id);
            return File(response.Contents, response.FileType, response.FileName);
        }

        [HttpPost]
        public async Task<IActionResult> UploadToDatabase(List<IFormFile> files, string description)
        {
            await fileDatabaseService.UploadToDatabaseAsync(files, description);
            TempData["Message"] = "File successfully uploaded to Database";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DownloadFileFromDatabase(int id)
        {
            var response = await fileDatabaseService.DownloadFileFromDatabase(id);
            return File(response.Contents, response.FileType, response.FileName);
        }

        //[HttpDelete]
        public async Task<IActionResult> DeleteFileFromDatabase(int id)
        {
            await fileDatabaseService.DeleteFileFromDatabase(id);
            TempData["Message"] = $"Removed successfully from Database.";
            return RedirectToAction("Index");
        }

        //[HttpDelete]
        public async Task<IActionResult> DeleteFileFromFileSystem(int id)
        {
            await fileSystemService.DeleteFileFromFileSystem(id);
            TempData["Message"] = $"Removed successfully from File System.";
            return RedirectToAction("Index");
        }
    }
}
