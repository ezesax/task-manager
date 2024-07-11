using System.Web.Mvc;
using Newtonsoft.Json;  // Asegúrate de que Newtonsoft.Json esté instalado

namespace TaskManagerAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var response = new
            {
                APIVersion = "1.0.0"
            };

            // Retorna un JSON con la versión de la API
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
