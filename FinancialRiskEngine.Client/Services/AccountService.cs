using Newtonsoft.Json;
using System.Net.Http;
using static System.Net.WebRequestMethods;

namespace FinancialRiskEngine.Client.Services
{
    public class AccountService
    {
        private readonly HttpClient _httpClient;

        public AccountService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FinancialRiskEngine.ServerAPI");
        }
    }
}
