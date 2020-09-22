namespace Files.Tutorial.Models
{
    public class FileDownloadModel
    {
        public string FileType { get; set; }

        public string FileName { get; set; }

        public byte[] Contents { get; set; }
    }
}