using System;
using SystemTasks = System.Threading.Tasks;
using TaskManagerAPI.Models;
using System.Data.Entity.Infrastructure;

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

        public async SystemTasks.Task SaveCryptoPriceAsync(string cryptoId, string currency)
        {
            try
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
            catch (DbUpdateException dbEx)
            {
                // Capturar detalles específicos del DbUpdateException
                Console.WriteLine("DbUpdateException: Error al actualizar la base de datos.");
                foreach (var entry in dbEx.Entries)
                {
                    Console.WriteLine($"Entity Type: {entry.Entity.GetType().Name}, State: {entry.State}");
                }
                Console.WriteLine($"Error Message: {dbEx.Message}");
                Console.WriteLine($"Inner Exception: {dbEx.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                // Capturar cualquier otra excepción
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
            }
        }
    }
}
