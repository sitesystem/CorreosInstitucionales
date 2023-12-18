using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using CorreosInstitucionales.Server.CapaDataAccess.DBContext;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
using CorreosInstitucionales.Shared.CapaTools;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UsuariosController : ControllerBase
    {
        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllData(bool filterByStatus) 
        {
            Response<List<MceTbUsuario>> oResponse = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();
                var list = new List<MceTbUsuario>();

                if (filterByStatus)
                    list = await db.MceTbUsuarios.Where(u => u.UsuStatus.Equals(filterByStatus)).ToListAsync();
                                            //   .OrderByDescending(x => x.Id)
                                            //   .Skip((actualPage - 1) * Utilities.REGISTERSPERPAGE)
                                            //   .Take(Utilities.REGISTERSPERPAGE)
                                            //   .ToList();
                else
                    list = await db.MceTbUsuarios.ToListAsync();

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
            Response<MceTbUsuario> oResponse = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();
                var list = await db.MceTbUsuarios.FindAsync(id);
                oResponse.Success = 1;
                oResponse.Data = list;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpGet("filterByNombre&CURP/{nombre}")]
        public async Task<IActionResult> GetDataByNombre(string correo, string curp)
        {
            Response<MceTbUsuario> oResponse = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();
                var list = await db.MceTbUsuarios.Where(vistaCorreo => vistaCorreo.UsuCorreoPersonalCuentaNueva == correo).FirstOrDefaultAsync();                
                var list2 = await db.MceTbUsuarios.Where(vistaCurp => vistaCurp.UsuCurp == curp).FirstOrDefaultAsync();
                if (list==null || list2==null)
                {
                    oResponse.Success = 0;
                    oResponse.Data = list;
                    oResponse.Data = list2;
                }
                else    
                {
                    oResponse.Success = 1;                    
                }
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpPost]
        public async Task<IActionResult> AddData(UsuarioViewModel model)
        {
            Response<object> oResponse = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();

                MceTbUsuario oUsuario = new()
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

                await db.MceTbUsuarios.AddAsync(oUsuario);
                await db.SaveChangesAsync();

                oResponse.Success = 1;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpPut]
        public async Task<IActionResult> EditData(UsuarioViewModel model)
        {
            Response<object> oRespuesta = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();

                MceTbUsuario? oUsuario = await db.MceTbUsuarios.FindAsync(model.IdUsuario);

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

                    db.Entry(oUsuario).State = EntityState.Modified;
                    await db.SaveChangesAsync();
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
            Response<MceTbUsuario> oRespuesta = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();

                MceTbUsuario? oUsuario = await db.MceTbUsuarios.Where(u => u.UsuCorreoPersonalCuentaNueva == correoPersonal).FirstOrDefaultAsync();

                if (oUsuario != null)
                {
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-_=+";
                    string tmpPassword = new(Enumerable.Repeat(chars, 10).Select(s => s[new Random().Next(s.Length)]).ToArray());

                    oUsuario.UsuContraseña = Encrypt.GetSHA256(tmpPassword);
                    oUsuario.UsuRecuperarContraseña = true;
                    db.Entry(oUsuario).State = EntityState.Modified;
                    await db.SaveChangesAsync();

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
                using DbCorreosInstUpiicsaContext db = new();

                MceTbUsuario? oUsuario = await db.MceTbUsuarios.FindAsync(id);

                if (oUsuario != null)
                {
                    oUsuario.UsuContraseña = Encrypt.GetSHA256(newPassword);
                    oUsuario.UsuRecuperarContraseña = false;
                    db.Entry(oUsuario).State = EntityState.Modified;
                    await db.SaveChangesAsync();
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
                using DbCorreosInstUpiicsaContext db = new();

                MceTbUsuario? oUsuario = await db.MceTbUsuarios.FindAsync(id);
                //db.Remove(oPersona);

                if (oUsuario != null)
                {
                    oUsuario.UsuStatus = isActivate;
                    db.Entry(oUsuario).State = EntityState.Modified;
                    await db.SaveChangesAsync();
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
