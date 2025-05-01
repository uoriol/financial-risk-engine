using static System.Net.WebRequestMethods;
using System.Collections;
using FinancialRiskEngine.Client.Utils.Managers;
using Microsoft.JSInterop;

namespace FinancialRiskEngine.Client.Services
{
    public class FileService
    {
        private readonly HttpClient _httpClient;
        public FileService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FinancialRiskEngine.ServerAPI");
        }

        public async Task DownloadBlob(string FileName, string ContainerName, IJSRuntime _jsRuntime)
        {
            var ht = new Hashtable();
            ht["blobName"] = FileName;
            ht["folder"] = ContainerName;
            await FileManager.DownloadWithPostAndSaveAsync(_jsRuntime, _httpClient, ht, "api/File/download-blob", FileName);
        }
    }
}
