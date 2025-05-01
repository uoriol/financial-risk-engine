using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Devices.Shared;

namespace FinancialRiskEngine.ServiceRepository.Implemetations
{
    public class BlobService
    {
        private string connection;

        public BlobService(IConfiguration configuration)
        {
            connection = "yourconnectionhere";
        }

        public BlobService() { 
            connection = "yourconnectionhere"; // or better yet use env variables
        }

        public async Task CreateContainer(string containerName)
        {
            var containerClient = new BlobContainerClient(connection, containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);
        }

        public byte[] DownloadBlobFile(string blobName, string containerName)
        {
            // If it doesn't work, we can try blobname + ".pdf"
            blobName = $"{blobName}.pdf";
            var containerClient = new BlobContainerClient(connection, containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            var stream = new MemoryStream();
            blobClient.DownloadTo(stream);
            var content = stream.ToArray();
            return content;
        }

        public async Task SaveFileAsync(byte[] fileBytes, string containerName, string fileName, IDictionary<string, string>? metadata = null, bool overrideExistingFile = true)
        {
            var blobName = $"{fileName}";
            var containerClient = new BlobContainerClient(connection, containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            using var stream = new MemoryStream(fileBytes);

            if (overrideExistingFile)
            {
                await blobClient.DeleteIfExistsAsync();
            }
            // Upload the file
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = "application/pdf" });

            if (metadata != null)
            {
                await blobClient.SetMetadataAsync(metadata);
            }
        }

        public IFormFile GetIFormFileFromByteArray(byte[] byteArray, string fileType = "pdf", string fileName = "test.pdf")
        {
            var formFile = new FormFile(new MemoryStream(byteArray), 0, byteArray.Length, fileType, fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = $"application/{fileType}"
            };
            return formFile;
        }
    }
}
