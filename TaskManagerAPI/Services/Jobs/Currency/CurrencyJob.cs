using Quartz;
using System;
using System.Threading.Tasks;

namespace TaskManagerAPI.Services.Jobs
{
    public class CurrencyJob : IJob
    {
        private readonly CurrencyService _currencyService;

        public CurrencyJob()
        {
            _currencyService = new CurrencyService();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            // Agrega un log al iniciar el job
            Console.WriteLine("CurrencyJob: Iniciando la ejecución del job...");

            await _currencyService.SaveCryptoPriceAsync("bitcoin", "usd");

            // Agrega un log al finalizar el job
            Console.WriteLine("CurrencyJob: Ejecución del job completada.");
        }
    }
}
