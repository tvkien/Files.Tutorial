using System.IO;
using System.Threading.Tasks;

namespace Files.Tutorial.Handlers
{
    public interface IFileAzureBlobService
    {
        Task UploadFromStreamAsync(Stream source);
    }
}