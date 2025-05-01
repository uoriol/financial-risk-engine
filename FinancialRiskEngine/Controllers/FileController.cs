using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Text.Json;
using FinancialRiskEngine.ServiceRepository.Implemetations;
using FinancialRiskEngine.ServiceRepository.Interfaces;

namespace FinancialRiskEngine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly BlobService _blobService;

        public FileController(BlobService blobService)
        {
            _blobService = blobService;
        }

        [HttpPost("download-blob")]
        public FileResult DownloadBlob(Hashtable ht)
        {
            var blobNameElement = ht["blobName"];
            var folderElement = ht["folder"];

            string blobName = blobNameElement is JsonElement je1 ? je1.GetString()! : blobNameElement.ToString();
            string folder = folderElement is JsonElement je2 ? je2.GetString()! : folderElement.ToString();
            var content = _blobService.DownloadBlobFile(blobName, folder);
            return File(content, "application/pdf", $"{blobName}.pdf");
        }
    }
}
