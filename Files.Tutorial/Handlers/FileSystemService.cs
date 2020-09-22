using Files.Tutorial.EF;
using Files.Tutorial.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Files.Tutorial.Handlers
{
    public class FileSystemService : IFileSystemService
    {
        private readonly ApplicationDbContext context;

        public FileSystemService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task DeleteFileFromFileSystem(int id)
        {
            var file = await context.FileOnFileSystem.FirstOrDefaultAsync(x => x.Id == id);
            if (file == null)
            {
                return;
            }

            if (File.Exists(file.FilePath))
            {
                File.Delete(file.FilePath);
            }

            context.FileOnFileSystem.Remove(file);
            await context.SaveChangesAsync();
        }

        public async Task<FileDownloadModel> DownloadFileFromFileSystem(int id)
        {
            var file = await context.FileOnFileSystem.FirstOrDefaultAsync(x => x.Id == id);
            if (file == null)
            {
                return null;
            }

            using var memory = new MemoryStream();
            using (var stream = new FileStream(file.FilePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return new FileDownloadModel
            {
                FileType = file.FileType,
                FileName = file.Name + file.Extension,
                Contents = memory.ToArray()
            };
        }

        public async Task<FileUploadViewModel> LoadAllFilesAsync()
        {
            var viewModel = new FileUploadViewModel
            {
                FilesOnDatabase = await context.FileOnDatabase.ToListAsync(),
                FilesOnFileSystem = await context.FileOnFileSystem.ToListAsync()
            };
            return viewModel;
        }

        public async Task UploadToFileSystemAsync(List<IFormFile> files, string description)
        {
            foreach (var file in files)
            {
                var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Files\\");
                var basePathExists = Directory.Exists(basePath);

                if (!basePathExists)
                {
                    Directory.CreateDirectory(basePath);
                }
                
                var filePath = Path.Combine(basePath, file.FileName);
                
                if (File.Exists(filePath))
                {
                    continue;
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                var fileModel = new FileOnFileSystemModel
                {
                    CreatedOn = DateTime.UtcNow,
                    FileType = file.ContentType,
                    Extension = extension,
                    Name = fileName,
                    Description = description,
                    FilePath = filePath
                };
                await context.FileOnFileSystem.AddAsync(fileModel);
                await context.SaveChangesAsync();
            }
        }
    }
}