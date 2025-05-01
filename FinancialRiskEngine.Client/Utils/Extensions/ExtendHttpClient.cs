using Newtonsoft.Json;
using System.Text;

namespace FinancialRiskEngine.Client.Utils.Extensions
{
    public static class ExtendHttpClient
    {
        public static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            MaxDepth = 256
        };

        /// <summary>
        /// Method to get objects analog to GetFromJsonAsync but using the Newtonsoft deserializer.
        /// </summary>
        /// <typeparam name="T">Type to which you want to deserialize the response of the server.</typeparam>
        /// <param name="url">Url of the request.</param>
        /// <returns>Deserializer object of type T.</returns>
        public static async Task<T> GetObjectsAsync<T>(this HttpClient http, string url)
        {
            var response = await http.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json, settings);
            }
            else
            {
                throw new HttpRequestException($"The response has status code {response.StatusCode}");
            }
        }

        /// <summary>
        /// Method to post objects analog to PostAsJsonAsync but using the Newtonsoft serializer.
        /// </summary>
        /// <param name="url">Url where the post is made.</param>
        /// <param name="objects">Objects that you want to post.</param>
        /// <returns>The resonse message from the server.</returns>
        public static async Task<HttpResponseMessage> PostObjectsAsync<T>(this HttpClient http, string url, T objects)
        {
            var json = JsonConvert.SerializeObject(objects, settings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await http.PostAsync(url, content);
            return response;
        }

        public static async Task<HttpResponseMessage> PostObjectsAsyncFile<T>(this HttpClient http, string url, T objects)
        {
            var json = JsonConvert.SerializeObject(objects, settings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await http.PostAsync(url, content);
            return response;
        }

        /// <summary>
        /// Method to post an object and deserialize the response to another object type. All using Newtonsoft serializer.
        /// </summary>
        /// <typeparam name="T">Type that you want the response to be deserialized to.</typeparam>
        /// <typeparam name="U">Type of the object sent (not necessary).</typeparam>
        /// <param name="http">HttpClient.</param>
        /// <param name="url">Url where the post is made.</param>
        /// <param name="objects">Objects that you want to post.</param>
        /// <returns></returns>
        public static async Task<T> PostAndGetObjectAsync<T, U>(this HttpClient http, string url, U objects)
        {
            var response = await http.PostObjectsAsync(url, objects);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content, settings);
            }
            else
            {
                throw new HttpRequestException($"The response has status code {response.StatusCode}");
            }
        }
    }
}
