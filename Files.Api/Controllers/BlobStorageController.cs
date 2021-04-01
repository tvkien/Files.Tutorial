﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Files.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobStorageController : ControllerBase
    {
        private readonly IOptions<MyConfig> config;

        public BlobStorageController(IOptions<MyConfig> config)
        {
            this.config = config;
        }

        [HttpPost("InsertFile")]
        public async Task<bool> InsertFileAsync(IFormFile asset)
        {


            try
            {
                if (CloudStorageAccount.TryParse(config.Value.StorageConnection, out CloudStorageAccount storageAccount))
                {
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobClient.GetContainerReference(config.Value.Container);
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(asset.FileName);
                    await blockBlob.UploadFromStreamAsync(asset.OpenReadStream());

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [HttpGet("ListFiles")]
        public async Task<List<string>> ListFiles()
        {
            List<string> blobs = new List<string>();
            try
            {
                if (CloudStorageAccount.TryParse(config.Value.StorageConnection, out CloudStorageAccount storageAccount))
                {
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                    CloudBlobContainer container = blobClient.GetContainerReference(config.Value.Container);

                    BlobResultSegment resultSegment = await container.ListBlobsSegmentedAsync(null);
                    foreach (IListBlobItem item in resultSegment.Results)
                    {
                        if (item.GetType() == typeof(CloudBlockBlob))
                        {
                            CloudBlockBlob blob = (CloudBlockBlob)item;
                            blobs.Add(blob.Name);
                        }
                        else if (item.GetType() == typeof(CloudPageBlob))
                        {
                            CloudPageBlob blob = (CloudPageBlob)item;
                            blobs.Add(blob.Name);
                        }
                        else if (item.GetType() == typeof(CloudBlobDirectory))
                        {
                            CloudBlobDirectory dir = (CloudBlobDirectory)item;
                            blobs.Add(dir.Uri.ToString());
                        }
                    }
                }
            }
            catch
            {
            }
            return blobs;
        }

        [HttpPost("Upload")]
        public async Task<bool> Upload()
        {
            try
            {
                if (CloudStorageAccount.TryParse(config.Value.StorageConnection, out CloudStorageAccount storageAccount))
                {
                    var contentType = Request.ContentType;
                    //var filename = Request.Headers[HeaderNames.ContentDisposition].ToString().Split("filename=")[1];
                    var filename = string.Empty;
                    if (contentType == "application/octet-stream")
                    {

                        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                        if (string.IsNullOrEmpty(filename)) filename = "filenameDefault.test";
                        CloudBlobContainer container = blobClient.GetContainerReference(config.Value.Container);
                        CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
                        using (var reader = new StreamReader(Request.Body))
                        {
                            await blockBlob.UploadFromStreamAsync(reader.BaseStream);
                            return true;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }


        }

        [HttpGet("DownloadFile/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            MemoryStream ms = new MemoryStream();
            if (CloudStorageAccount.TryParse(config.Value.StorageConnection, out CloudStorageAccount storageAccount))
            {
                CloudBlobClient BlobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = BlobClient.GetContainerReference(config.Value.Container);

                if (await container.ExistsAsync())
                {
                    CloudBlob file = container.GetBlobReference(fileName);

                    if (await file.ExistsAsync())
                    {
                        await file.DownloadToStreamAsync(ms);
                        Stream blobStream = file.OpenReadAsync().Result;
                        return File(blobStream, file.Properties.ContentType, file.Name);
                    }
                    else
                    {
                        return Content("File does not exist");
                    }
                }
                else
                {
                    return Content("Container does not exist");
                }
            }
            else
            {
                return Content("Error opening storage");
            }
        }

        [Route("DeleteFile/{fileName}")]
        [HttpGet]
        public async Task<bool> DeleteFile(string fileName)
        {
            try
            {
                if (CloudStorageAccount.TryParse(config.Value.StorageConnection, out CloudStorageAccount storageAccount))
                {
                    CloudBlobClient BlobClient = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = BlobClient.GetContainerReference(config.Value.Container);

                    if (await container.ExistsAsync())
                    {
                        CloudBlob file = container.GetBlobReference(fileName);

                        if (await file.ExistsAsync())
                        {
                            await file.DeleteAsync();
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}