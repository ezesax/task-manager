using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace TaskManagerAPI.Services
{
    [Authorize]
    public class CryptoService
    {
        private readonly HttpClient _httpClient;

        public CryptoService()
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
        }

        public async Task<string> GetCryptoPriceAsync(string cryptoId)
        {
            var url = $"https://api.coingecko.com/api/v3/simple/price?ids={cryptoId}&vs_currencies=usd";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }

                return "Unable to get cryptocurrency data";
            }
            catch (Exception ex)
            {
                return $"An error occurred: {ex.Message}";
            }
        }
    }
}
