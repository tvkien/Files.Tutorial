using Azure.Storage.Blobs;
using Files.Tutorial.EF;
using System.IO;
using System.Threading.Tasks;

namespace Files.Tutorial.Handlers
{
    public class FileAzureBlobService : IFileAzureBlobService
    {
        private readonly ApplicationDbContext context;

        public FileAzureBlobService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Task UploadFromStreamAsync(Stream source)
        {
            throw new System.NotImplementedException();
        }

        //public Task UploadFromStreamAsync(Stream source)
        //{
        //    // Get a connection string to our Azure Storage account.
        //    string connectionString = "<connection_string>";
        //    string containerName = "sample-container";
        //    string blobName = "sample-blob";
        //    string filePath = "hello.jpg";

        //    // Get a reference to a container named "sample-container" and then create it
        //    BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
        //    container.Create();
        //    BlobClient blob = container.GetBlobClient(blobName);
        //}
    }
}
