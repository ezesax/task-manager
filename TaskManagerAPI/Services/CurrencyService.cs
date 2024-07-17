using System;
using System.Threading.Tasks;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services
{
    public class CurrencyService
    {
        private readonly DBContext _context;
        private readonly CryptoService _cryptoService;

        public CurrencyService()
        {
            _context = new DBContext();
            _cryptoService = new CryptoService();
        }

        public async System.Threading.Tasks.Task SaveCryptoPriceAsync(string cryptoId, string currency)
        {
            var jsonResult = await _cryptoService.GetCryptoPriceAsync(cryptoId);
            if (jsonResult == null)
            {
                throw new Exception("Unable to get cryptocurrency data");
            }

            var priceData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonResult);
            var price = (decimal)priceData[cryptoId][currency];

            var currencyRecord = new Currency
            {
                Coin = cryptoId,
                Currency_type = currency,
                Price = price,
                Checked = DateTime.UtcNow
            };

            _context.Currencies.Add(currencyRecord);
            await _context.SaveChangesAsync();
        }
    }
}
