using System.Collections.Generic;

namespace Files.Tutorial.Models
{
    public class FileUploadViewModel
    {
        public List<FileOnFileSystemModel> FilesOnFileSystem { get; set; }

        public List<FileOnDatabaseModel> FilesOnDatabase { get; set; }
    }
}