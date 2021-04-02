using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Net.Http.Headers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Files.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpoController : ControllerBase
    {
        private readonly IOptions<MyConfig> config;
        private static IGraphServiceClient graphServiceClient;

        public SpoController(IOptions<MyConfig> config)
        {
            this.config = config;
        }

        [HttpPost("Upload")]
        public async Task<bool> Upload()
        {
            try
            {
                var contentType = Request.ContentType;
                //var filename = Request.Headers[HeaderNames.ContentDisposition].ToString().Split("filename=")[1];
                var filename = string.Empty;
                if (contentType == "application/octet-stream")
                {

                    //CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                    //if (string.IsNullOrEmpty(filename)) filename = "filenameDefault.test";
                    //CloudBlobContainer container = blobClient.GetContainerReference(config.Value.Container);
                    //CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
                    //using (var reader = new StreamReader(Request.Body))
                    //{
                    //    await blockBlob.UploadFromStreamAsync(reader.BaseStream);
                    //    return true;
                    //}

                    var token = await GetAccessTokenAsync();
                    graphServiceClient = new GraphServiceClient(
                                   new DelegateAuthenticationProvider(
                                       async (requestMessage) =>
                                       {
                                           requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                       }));

                    var site = @"https://pod3.sharepoint.com/sites/AuvenirDev_Local_-55c9bfd7-c7ee-4e2b-b7f1-f24a01e7b4ca/944e205f-9272-4e4d-aa6a-8d9702e5e0b4";

                    using (var reader = new StreamReader(Request.Body))
                    {
                        await UploadLargeFileAsync(site, reader.BaseStream);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }

        private static async Task<string> GetAccessTokenAsync()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                var requestData = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("client_id", "af33b34f-06de-4f7c-97fb-7a01e93064d4"),
                    new KeyValuePair<string, string>("client_secret", ".WDibIBNEObf.bHl-o0N9xD6.DzN3zN5lK"),
                    new KeyValuePair<string, string>("scope", "https://graph.microsoft.com/.default"),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", "dev@pod3.onmicrosoft.com"),
                    new KeyValuePair<string, string>("password", "12345678x@X")
                };

                var httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri("https://login.microsoftonline.com/a36e627f-33eb-4ab9-9e01-653019cc7b55/oauth2/v2.0/token"),
                    Method = HttpMethod.Post,
                    Content = new FormUrlEncodedContent(requestData)
                };

                var response = await httpClient.SendAsync(httpRequestMessage);
                var dataResponse = await response.Content.ReadAsStringAsync();
                var tokenResult = JsonSerializer.Deserialize<JsonElement>(dataResponse);
                return tokenResult.GetProperty("access_token").GetString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task UploadLargeFileAsync(string siteUrl, Stream content)
        {
            var uriSite = new Uri(siteUrl);
            var siteCollection = await graphServiceClient.Sites.GetByPath(uriSite.AbsolutePath, uriSite.Host).Request().GetAsync();
            var drive = graphServiceClient.Sites[siteCollection.Id].Drive.Root;

            var uploadProps = new DriveItemUploadableProperties
            {
                ODataType = null,
                AdditionalData = new Dictionary<string, object>
                {
                    { "@microsoft.graph.conflictBehavior", "replace" }
                }
            };

            var uploadSession = await drive
              .ItemWithPath("Engagement Request File/English.docx")
              .CreateUploadSession(uploadProps)
              .Request()
              .PostAsync();
            int maxSliceSize = 320 * 1024;
            var fileUploadTask = new LargeFileUploadTask<DriveItem>(uploadSession, content, maxSliceSize);
            IProgress<long> progress = new Progress<long>(progress => {
                Console.WriteLine($"Uploaded {progress} bytes of {content.Length} bytes");
            });

            try
            {
                var uploadResult = await fileUploadTask.UploadAsync(progress);

                if (uploadResult.UploadSucceeded)
                {
                    Console.WriteLine($"Upload complete, item ID: {uploadResult.ItemResponse.Id}");
                }
                else
                {
                    Console.WriteLine("Upload failed");
                }
            }
            catch (ServiceException ex)
            {
                Console.WriteLine($"Error uploading: {ex.ToString()}");
            }
        }
    }
}