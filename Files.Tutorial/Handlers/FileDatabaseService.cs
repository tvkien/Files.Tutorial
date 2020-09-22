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
    public class FileDatabaseService : IFileDatabaseService
    {
        private readonly ApplicationDbContext context;

        public FileDatabaseService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task DeleteFileFromDatabase(int id)
        {
            var file = await context.FileOnDatabase.FirstOrDefaultAsync(x => x.Id == id);
            context.FileOnDatabase.Remove(file);
            await context.SaveChangesAsync();
        }

        public async Task<FileDownloadModel> DownloadFileFromDatabase(int id)
        {
            var file = await context.FileOnDatabase.FirstOrDefaultAsync(x => x.Id == id);

            return file == null
                ? new FileDownloadModel()
                : new FileDownloadModel
            {
                Contents = file.Data,
                FileType = file.FileType,
                FileName = file.Name + file.Extension
            };
        }

        public async Task UploadToDatabaseAsync(List<IFormFile> files, string description)
        {
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                var fileModel = new FileOnDatabaseModel
                {
                    CreatedOn = DateTime.UtcNow,
                    FileType = file.ContentType,
                    Extension = extension,
                    Name = fileName,
                    Description = description
                };
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    fileModel.Data = dataStream.ToArray();
                }
                await context.FileOnDatabase.AddAsync(fileModel);
                await context.SaveChangesAsync();
            }
        }
    }
}
