using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using CorreosInstitucionales.Server.CapaDataAccess.DBContext;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request.Login;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
using CorreosInstitucionales.Shared.CapaTools;

namespace CorreosInstitucionales.Server.CapaDataAccess.LoginAuth
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "USUARIOMAESTRO,ADMINISTRADOR")]
    public class LoginController : ControllerBase
    {
        private ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginAuthViewModel model)
        {
            Response<LoginUsuarioResponse> oResponse = new();

            var usuarioResponse = _loginService.Auth(model);

            if (usuarioResponse == null || usuarioResponse.IdUsuario == 0)
            {
                oResponse.Success = 0;
                oResponse.Message = "Usuario y/o contraseña incorrecta o inhabilitado";
                return BadRequest(oResponse);
            }

            oResponse.Success = 1;
            oResponse.Message = "Usuario y contraseña correctos";
            oResponse.Data = usuarioResponse;

            return Ok(usuarioResponse.UsuToken);
        }
    }
}
