using System.Threading.Tasks;
using System.Web.Http;
using TaskManagerAPI.Services;

namespace TaskManagerAPI.Controllers
{
    [Authorize]
    public class CryptoController : ApiController
    {
        private readonly CryptoService _cryptoService;

        public CryptoController()
        {
            _cryptoService = new CryptoService();
        }

        // GET: api/crypto/bitcoin
        [HttpGet]
        [Route("api/crypto/{cryptoId}")]
        public async Task<IHttpActionResult> GetCryptoPrice(string cryptoId)
        {
            var jsonResult = await _cryptoService.GetCryptoPriceAsync(cryptoId);
            return Ok(Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResult));
        }
    }
}
