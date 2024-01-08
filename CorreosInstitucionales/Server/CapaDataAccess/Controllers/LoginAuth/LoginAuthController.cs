using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers.LoginAuth
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "USUARIOMAESTRO,ADMINISTRADOR")]
    public class LoginAuthController(ILoginAuthService loginAuthService) : ControllerBase
    {
        private readonly ILoginAuthService _loginAuthService = loginAuthService;

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] RequestDTO_LoginAuth model)
        {
            Response<ResponseDTO_LoginUsuario> oResponse = new();

            var usuarioResponse = await _loginAuthService.Auth(model);

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
