using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using CorreosInstitucionales.Shared.CapaTools;
using CorreosInstitucionales.Shared.CapaDataAccess;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class DebugController : Controller
    {
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("auth")]
        public IActionResult Prueba()
        {
            return Ok("Prueba");
        }

        [HttpGet("cache")]
        public IActionResult CacheData()
        {
            return Ok($"PLANTILLAS: {AppCache.Plantillas.Length}");
        }
    }
}
