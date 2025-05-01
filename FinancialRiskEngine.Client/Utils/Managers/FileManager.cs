using Microsoft.JSInterop;
using FinancialRiskEngine.Client.Utils.Extensions;

namespace FinancialRiskEngine.Client.Utils.Managers
{
    public static class FileManager
    {

        /// <summary>
        /// Downloads the file in the browser with the given name using a POST method.
        /// </summary>
        /// <param name="jsRuntime">IJSRuntime object injected to the component.</param>
        /// <param name="fileContent">Stream with the file content (usually from an http response).</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        private static async Task SaveAsync(IJSRuntime jsRuntime, Stream fileContent, string fileName)
        {
            var ms = new MemoryStream();
            fileContent.CopyTo(ms);
            await jsRuntime.InvokeAsync<object>("FileSaveAs", fileName, Convert.ToBase64String(ms.ToArray()));
        }

        /// <summary>
        /// Posts an object to the given url, retrieve the file that the server returns and download it in the client browser.
        /// </summary>
        /// <typeparam name="T">Not necessary.</typeparam>
        /// <param name="jsRuntime">IJSRuntime object injected to the component.</param>
        /// <param name="http">HttpClient used to make the request. Usually injected in the component.</param>
        /// <param name="post">Objects to be posted.</param>
        /// <param name="url">Url where the post is made.</param>
        /// <param name="fileName">Name of the file that the client will see.</param>
        /// <returns></returns>
        public static async Task DownloadWithPostAndSaveAsync<T>(IJSRuntime jsRuntime, HttpClient http, T post, string url, string fileName)
        {
            var response = await http.PostObjectsAsync(url, post);
            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                await FileManager.SaveAsync(jsRuntime, stream, fileName);
            }
            else
            {
                throw new HttpRequestException($"The response has status code {response.StatusCode}");
            }
        }

        /// <summary>
        /// Downloads a file in the browser using a GET method.
        /// </summary>
        /// <param name="jsRuntime">IJSRuntime object injected to the component.</param>
        /// <param name="http">HttpClient used to make the request. Usually injected in the component.</param>
        /// <param name="url">Url where the post is made.</param>
        /// <param name="fileName">Name of the file that the client will see.</param>
        /// <returns></returns>
        public static async Task DownloadAndSaveAsync(IJSRuntime jsRuntime, HttpClient http, string url, string fileName)
        {
            var response = await http.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                await FileManager.SaveAsync(jsRuntime, stream, fileName);
            }
            else
            {
                throw new HttpRequestException($"The response has status code {response.StatusCode}");
            }
        }
    }
}
