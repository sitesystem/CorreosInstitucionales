using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using CorreosInstitucionales.Shared.CapaTools;
using System;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers.MóduloRegistros
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UsuariosController(DbCorreosInstitucionalesUpiicsaContext db, IHostEnvironment hostEnvironment, IWebHostEnvironment webHostEnvironment) : ControllerBase
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;
        private readonly IHostEnvironment _hostEnvironment = hostEnvironment;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllData(bool filterByStatus)
        {
            Response<List<MpTbUsuario>> oResponse = new();

            try
            {
                var list = new List<MpTbUsuario>();

                if (filterByStatus)
                    list = await _db.MpTbUsuarios.Where(u => u.UsuStatus.Equals(filterByStatus)).ToListAsync();
                                    //   .OrderByDescending(x => x.Id)
                                    //   .Skip((actualPage - 1) * Utilities.REGISTERSPERPAGE)
                                    //   .Take(Utilities.REGISTERSPERPAGE)
                                    //   .ToList();
                else
                    list = await _db.MpTbUsuarios.ToListAsync();

                oResponse.Success = 1;
                oResponse.Data = list;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }
            return Ok(oResponse);
        }

        [HttpGet("filterById/{id}")]
        public async Task<IActionResult> GetDataById(int id)
        {
            Response<MpTbUsuario> oResponse = new();

            try
            {
                var item = await _db.MpTbUsuarios.FindAsync(id);
                oResponse.Success = 1;
                oResponse.Data = item;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpGet("filterByEmailCURP/{correo}/{curp}")]
        public async Task<IActionResult> ValidateByEmailCURP(string correo, string curp)
        {
            Response<MpTbUsuario> oResponse = new();

            try
            {
                var list = await _db.MpTbUsuarios
                                    .Where(u => u.UsuCorreoPersonalCuentaNueva == correo || u.UsuCurp == curp)
                                    .FirstOrDefaultAsync();
                if (list == null)
                    oResponse.Success = 1;
                else
                    oResponse.Data = list;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpPost]
        public async Task<IActionResult> AddData(RequestDTO_Usuario model)
        {
            Response<object> oResponse = new();
            CreatedAtActionResult oCreatedAtActionResult;

            try
            {
                MpTbUsuario oUsuario = new()
                {
                    // DATOS ID DEL USUARIO
                    IdUsuario = model.IdUsuario,
                    UsuIdRol = model.UsuIdRol,
                    UsuIdTipoPersonal = model.UsuIdTipoPersonal,
                    // UsuToken = Guid.NewGuid().ToString("D"),
                    // DATOS PERSONALES
                    UsuNombre = model.UsuNombre.ToUpper(),
                    UsuPrimerApellido = model.UsuPrimerApellido.ToUpper(),
                    UsuSegundoApellido = model.UsuSegundoApellido.ToUpper(),
                    UsuCurp = model.UsuCurp.ToUpper(),
                    UsuFileNameCurp = model.UsuFileNameCurp,
                    UsuNoCelularAnterior = null,
                    UsuNoCelularNuevo = model.UsuNoCelularNuevo,
                    // DATOS ACADÉMICOS
                    UsuBoletaAlumno = model.UsuBoletaAlumno,
                    UsuBoletaMaestria = model.UsuBoletaMaestria,
                    UsuIdCarrera = model.UsuIdCarrera,
                    UsuSemestre = model.UsuSemestre,
                    UsuAñoEgreso = model.UsuAñoEgreso,
                    UsuFileNameComprobanteInscripcion = model.UsuFileNameComprobanteInscripcion,
                    // DATOS LABORALES
                    UsuNumeroEmpleado = model.UsuNumeroEmpleado,
                    UsuIdAreaDepto = model.UsuIdAreaDepto,
                    UsuNoExtension = model.UsuNoExtension,
                    // DATOS DE LAS CREDENCIALES DE LA CUENTA EN LA APP
                    UsuCorreoPersonalCuentaAnterior = model.UsuCorreoPersonalCuentaAnterior,
                    UsuCorreoPersonalCuentaNueva = model.UsuCorreoPersonalCuentaNueva,
                    UsuContraseña = Encrypt.GetSHA256(model.UsuContraseña),
                    UsuRecuperarContraseña = false,
                    // DATOS DEL CORREO INSTITUCIONAL
                    UsuCorreoInstitucionalCuenta = model.UsuCorreoInstitucionalCuenta,
                    UsuCorreoInstitucionalContraseña = model.UsuCorreoInstitucionalContraseña,
                    // OTROS DATOS
                    UsuFechaHoraAlta = DateTime.Now,
                    UsuStatus = true,
                    // DATOS FK NAVIGATION
                    UsuIdAreaDeptoNavigation = null,
                    UsuIdCarreraNavigation = null,
                    UsuIdRolNavigation = null,
                    UsuIdTipoPersonalNavigation = null
                };

                await _db.MpTbUsuarios.AddAsync(oUsuario);
                await _db.SaveChangesAsync();

                oResponse.Success = 1;

                oCreatedAtActionResult = CreatedAtAction(nameof(AddData), new { id = oUsuario.IdUsuario }, oUsuario);
                oResponse.Message = oUsuario.IdUsuario.ToString(); // PK ID Único del Usuario Creado o dado de Alta
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpPut]
        public async Task<IActionResult> EditData(RequestDTO_Usuario model)
        {
            Response<object> oRespuesta = new();

            try
            {
                MpTbUsuario? oUsuario = await _db.MpTbUsuarios.FindAsync(model.IdUsuario);

                if (oUsuario != null)
                {
                    // DATOS ID DEL USUARIO
                    oUsuario.UsuIdRol = 2;
                    oUsuario.UsuIdTipoPersonal = model.UsuIdTipoPersonal;
                    // oUsuario.UsuToken = Guid.NewGuid().ToString("D"),
                    // DATOS PERSONALES
                    oUsuario.UsuNombre = model.UsuNombre.ToUpper();
                    oUsuario.UsuPrimerApellido = model.UsuPrimerApellido.ToUpper();
                    oUsuario.UsuSegundoApellido = model.UsuSegundoApellido.ToUpper();
                    oUsuario.UsuCurp = model.UsuCurp.ToUpper();
                    oUsuario.UsuFileNameCurp = model.UsuFileNameCurp;
                    oUsuario.UsuNoCelularAnterior = model.UsuNoCelularAnterior;
                    oUsuario.UsuNoCelularNuevo = model.UsuNoCelularNuevo;
                    // DATOS ACADÉMICOS
                    oUsuario.UsuBoletaAlumno = model.UsuBoletaAlumno;
                    oUsuario.UsuBoletaMaestria = model.UsuBoletaMaestria;
                    oUsuario.UsuIdCarrera = model.UsuIdCarrera;
                    oUsuario.UsuSemestre = model.UsuSemestre;
                    oUsuario.UsuAñoEgreso = model.UsuAñoEgreso;
                    oUsuario.UsuFileNameComprobanteInscripcion = model.UsuFileNameComprobanteInscripcion;
                    // DATOS LABORALES
                    oUsuario.UsuNumeroEmpleado = model.UsuNumeroEmpleado;
                    oUsuario.UsuIdAreaDepto = model.UsuIdAreaDepto;
                    oUsuario.UsuNoExtension = model.UsuNoExtension;
                    // DATOS DE LAS CREDENCIALES DE LA CUENTA EN LA APP
                    oUsuario.UsuCorreoPersonalCuentaAnterior = model.UsuCorreoPersonalCuentaAnterior;
                    oUsuario.UsuCorreoPersonalCuentaNueva = model.UsuCorreoPersonalCuentaNueva;
                    oUsuario.UsuContraseña = Encrypt.GetSHA256(model.UsuContraseña);
                    oUsuario.UsuRecuperarContraseña = false;
                    // DATOS DEL CORREO INSTITUCIONAL
                    oUsuario.UsuCorreoInstitucionalCuenta = model.UsuCorreoInstitucionalCuenta;
                    oUsuario.UsuCorreoInstitucionalContraseña = model.UsuCorreoInstitucionalContraseña;
                    // OTROS DATOS
                    // oUsuario.UsuFechaHoraAlta = DateTime.UtcNow;
                    // oUsuario.UsuStatus = true;
                    // DATOS FK NAVIGATION
                    oUsuario.UsuIdAreaDeptoNavigation = null;
                    oUsuario.UsuIdCarreraNavigation = null;
                    oUsuario.UsuIdRolNavigation = null;
                    oUsuario.UsuIdTipoPersonalNavigation = null;

                    _db.Entry(oUsuario).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                }

                oRespuesta.Success = 1;
            }
            catch (Exception ex)
            {
                oRespuesta.Message = ex.Message;
            }

            return Ok(oRespuesta);
        }

        [HttpPut("resetPassword/{correoPersonal}")]
        public async Task<IActionResult> ResetPassword(string correoPersonal)
        {
            Response<MpTbUsuario> oRespuesta = new();
            try
            {
                MpTbUsuario? oUsuario = await _db.MpTbUsuarios.Where(u => u.UsuCorreoPersonalCuentaNueva == correoPersonal).FirstOrDefaultAsync();

                if (oUsuario != null)
                {
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-_=+";
                    string tmpPassword = new(Enumerable.Repeat(chars, 10).Select(s => s[new Random().Next(s.Length)]).ToArray());

                    oUsuario.UsuContraseña = Encrypt.GetSHA256(tmpPassword);
                    oUsuario.UsuRecuperarContraseña = true;

                    _db.Entry(oUsuario).State = EntityState.Modified;
                    await _db.SaveChangesAsync();

                    oRespuesta.Success = 1;
                    oRespuesta.Message = $"Se generó una Contraseña Temporal: <b>{tmpPassword}</b>";
                    oRespuesta.Data = oUsuario;
                }
                else
                    oRespuesta.Message = "No se encuentró registrado el Correo Electrónico. Si el problema persisite acudir a la Unidad Informática (UDI).";
            }
            catch (Exception ex)
            {
                oRespuesta.Message = ex.Message;
            }

            return Ok(oRespuesta);
        }

        [HttpPut("changePassword/{id}/{newPassword}")]
        public async Task<IActionResult> ChangePassword(int id, string newPassword)
        {
            Response<object> oRespuesta = new();

            try
            {
                MpTbUsuario? oUsuario = await _db.MpTbUsuarios.FindAsync(id);

                if (oUsuario != null)
                {
                    oUsuario.UsuContraseña = Encrypt.GetSHA256(newPassword);
                    oUsuario.UsuRecuperarContraseña = false;

                    _db.Entry(oUsuario).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                }

                oRespuesta.Success = 1;
            }
            catch (Exception ex)
            {
                oRespuesta.Message = ex.Message;
            }

            return Ok(oRespuesta);
        }

        //[HttpDelete("{Id}")]
        [HttpPut("editByIdStatus/{id}/{isActivate}")]
        public async Task<IActionResult> EnableDisableDataById(int id, bool isActivate)
        {
            Response<object> oRespuesta = new();

            try
            {
                MpTbUsuario? oUsuario = await _db.MpTbUsuarios.FindAsync(id);
                //db.Remove(oPersona);

                if (oUsuario != null)
                {
                    oUsuario.UsuStatus = isActivate;
                    _db.Entry(oUsuario).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                }

                oRespuesta.Success = 1;
            }
            catch (Exception ex)
            {
                oRespuesta.Message = ex.Message;
            }

            return Ok(oRespuesta);
        }
    }
}
