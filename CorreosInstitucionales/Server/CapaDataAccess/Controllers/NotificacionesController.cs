using Microsoft.AspNetCore.Mvc;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]

    public class NotificacionesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
