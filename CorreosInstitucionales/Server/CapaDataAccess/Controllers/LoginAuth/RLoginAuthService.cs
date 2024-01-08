using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using CorreosInstitucionales.Shared.CapaEntities.Common;
using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using CorreosInstitucionales.Shared.CapaTools;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers.LoginAuth
{
    public class RLoginAuthService(IOptions<AppSettings> appSettings, DbCorreosInstitucionalesUpiicsaContext db) : ILoginAuthService
    {
        private readonly AppSettings _appSettings = appSettings.Value;
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;

        public async Task<ResponseDTO_LoginUsuario> Auth(RequestDTO_LoginAuth model)
        {
            ResponseDTO_LoginUsuario usuarioResponse = new();

            string spassword = Encrypt.GetSHA256(model.UsuContraseña);
            var usuario = await _db.MpTbUsuarios
                                   .Where(d => d.UsuCorreoPersonalCuentaNueva == model.UsuCorreoPersonal && d.UsuContraseña == spassword && d.UsuStatus.Equals(true))
                                   .FirstOrDefaultAsync();
            if (usuario != null)
            {
                // DATOS ID DEL USUARIO
                usuarioResponse.IdUsuario = usuario.IdUsuario;
                usuarioResponse.UsuIdRol = usuario.UsuIdRol;
                usuarioResponse.UsuIdTipoPersonal = usuario.UsuIdTipoPersonal;
                // DATOS PERSONALES
                usuarioResponse.UsuNombre = usuario.UsuNombre;
                usuarioResponse.UsuPrimerApellido = usuario.UsuPrimerApellido;
                usuarioResponse.UsuSegundoApellido = usuario.UsuSegundoApellido;
                usuarioResponse.UsuCurp = usuario.UsuCurp;
                usuarioResponse.UsuNoCelularAnterior = usuario.UsuNoCelularAnterior;
                usuarioResponse.UsuNoCelularNuevo = usuario.UsuNoCelularNuevo;
                // DATOS ACADÉMICOS
                usuarioResponse.UsuBoletaAlumno = usuario.UsuBoletaAlumno;
                usuarioResponse.UsuBoletaMaestria = usuario.UsuBoletaMaestria;
                usuarioResponse.UsuIdCarrera = usuario.UsuIdCarrera;
                usuarioResponse.UsuSemestre = usuario.UsuSemestre;
                usuarioResponse.UsuAñoEgreso = usuario.UsuAñoEgreso;
                // DATOS LABORALES
                usuarioResponse.UsuNumeroEmpleado = usuario.UsuNumeroEmpleado;
                usuarioResponse.UsuIdAreaDepto = usuario.UsuIdAreaDepto;
                usuarioResponse.UsuNoExtension = usuario.UsuNoExtension;
                // DATOS DE LAS CREDENCIALES DE LA CUENTA EN LA APP
                usuarioResponse.UsuCorreoPersonalCuentaAnterior = usuario.UsuCorreoPersonalCuentaAnterior;
                usuarioResponse.UsuCorreoPersonalCuentaNueva = usuario.UsuCorreoPersonalCuentaNueva;
                usuarioResponse.UsuRecuperarContraseña = usuario.UsuRecuperarContraseña;
                // DATOS DEL CORREO INSTITUCIONAL
                usuarioResponse.UsuCorreoInstitucionalCuenta = usuario.UsuCorreoInstitucionalCuenta;
                usuarioResponse.UsuCorreoInstitucionalContraseña = usuario.UsuCorreoInstitucionalContraseña;
                // OTROS DATOS
                usuarioResponse.UsuFechaHoraAlta = usuario.UsuFechaHoraAlta;
                usuarioResponse.UsuStatus = usuario.UsuStatus;
                usuarioResponse.UsuToken = GetToken(usuario);
            }

            return usuarioResponse;
        }

        private string GetToken(MpTbUsuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            //var key = Encoding.ASCII.GetBytes(_appSettings.Secreto);
            var key = Encoding.UTF8.GetBytes(_appSettings.Secreto ?? string.Empty);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = null,
                Audience = null,
                Subject = new ClaimsIdentity(
                        new Claim[]
                        {
                            new(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                            new(ClaimTypes.Name, usuario.UsuNombre.ToString()),
                            new(ClaimTypes.Role, usuario.UsuIdRol.ToString()),
                            new("ID", usuario.IdUsuario.ToString()),
                            new("Name", usuario.UsuNombre.ToString() + " " + usuario.UsuPrimerApellido.ToString() + " " + usuario.UsuSegundoApellido.ToString()),
                            new("Email", usuario.UsuCorreoPersonalCuentaNueva.ToString()),
                            new("Rol", usuario.UsuIdRol.ToString()),
                            new("TipoPersonal", usuario.UsuIdTipoPersonal.ToString())
                        }
                    ),
                Expires = DateTime.UtcNow.AddMonths(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
