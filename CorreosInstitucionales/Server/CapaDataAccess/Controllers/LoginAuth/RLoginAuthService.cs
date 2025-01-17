﻿using Microsoft.EntityFrameworkCore;
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

            string spassword = Encrypt.GetSHA256(model.UsuContrasenia);
            var usuario = await _db.MpTbUsuarios
                                   .Where(l => l.UsuCorreoPersonalCuentaActual == model.UsuCorreoPersonal && l.UsuContrasenia == spassword && l.UsuStatus.Equals(true))
                                   .FirstOrDefaultAsync();
            if (usuario != null)
            {
                // DATOS ID DEL USUARIO
                usuarioResponse.IdUsuario = usuario.IdUsuario;
                usuarioResponse.UsuIdRol = usuario.UsuIdRol;
                usuarioResponse.UsuIdTipoPersonal = usuario.UsuIdTipoPersonal;
                // DATOS PERSONALES
                usuarioResponse.UsuNombres = usuario.UsuNombres;
                usuarioResponse.UsuPrimerApellido = usuario.UsuPrimerApellido;
                usuarioResponse.UsuSegundoApellido = usuario.UsuSegundoApellido;
                usuarioResponse.UsuCurp = usuario.UsuCurp;
                usuarioResponse.UsuNoCelularAnterior = usuario.UsuNoCelularAnterior;
                usuarioResponse.UsuNoCelularActual = usuario.UsuNoCelularActual;
                // DATOS ACADÉMICOS
                usuarioResponse.UsuBoletaAlumnoEgresado = usuario.UsuBoletaAlumnoEgresado;
                usuarioResponse.UsuBoletaPosgrado = usuario.UsuBoletaPosgrado;
                usuarioResponse.UsuIdCarrera = usuario.UsuIdCarrera;
                usuarioResponse.UsuSemestre = usuario.UsuSemestre;
                usuarioResponse.UsuAnioEgreso = usuario.UsuAnioEgreso;
                // DATOS LABORALES
                usuarioResponse.UsuNumeroEmpleadoContrato = usuario.UsuNumeroEmpleadoContrato;
                usuarioResponse.UsuIdAreaDepto = usuario.UsuIdAreaDepto;
                usuarioResponse.UsuNoExtensionActual = usuario.UsuNoExtensionActual;
                // DATOS DE LAS CREDENCIALES DE LA CUENTA EN LA APP
                usuarioResponse.UsuCorreoPersonalCuentaAnterior = usuario.UsuCorreoPersonalCuentaAnterior;
                usuarioResponse.UsuCorreoPersonalCuentaActual = usuario.UsuCorreoPersonalCuentaActual;
                usuarioResponse.UsuRecuperarContrasenia = usuario.UsuRecuperarContrasenia;
                // DATOS DEL CORREO INSTITUCIONAL
                usuarioResponse.UsuCorreoInstitucionalCuenta = usuario.UsuCorreoInstitucionalCuenta;
                usuarioResponse.UsuCorreoInstitucionalContrasenia = usuario.UsuCorreoInstitucionalContrasenia;
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
            var key = Encoding.UTF8.GetBytes(_appSettings.Secreto ?? string.Empty); // var key = Encoding.ASCII.GetBytes(_appSettings.Secreto);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = null,
                Audience = null,
                Subject = new ClaimsIdentity
                (
                    [
                        new(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                        new(ClaimTypes.Name, usuario.UsuNombres.ToString()),
                        new(ClaimTypes.Role, usuario.UsuIdRol.ToString()),
                        new("ID", usuario.IdUsuario.ToString()),
                        new("Name", usuario.UsuNombres.ToString() + " " + usuario.UsuPrimerApellido.ToString() + " " + usuario.UsuSegundoApellido?.ToString()),
                        new("Email", usuario.UsuCorreoPersonalCuentaActual.ToString()),
                        new("Rol", usuario.UsuIdRol.ToString()),
                        new("TipoPersonal", usuario.UsuIdTipoPersonal.ToString()),
                        new("RecuperarContrasenia", usuario.UsuRecuperarContrasenia.ToString().ToLower())
                    ]
                ),
                Expires = DateTime.UtcNow.AddMonths(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
