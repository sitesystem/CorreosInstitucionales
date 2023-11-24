using CorreosInstitucionales.Server.CapaDataAccess.DBContext;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
using CorreosInstitucionales.Shared.CapaTools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllData(bool filterByStatus) 
        {
            Response<List<MceTbUsuario>> oResponse = new();

            try
            {
                using (DbCorreosInstUpiicsaContext db = new())
                {
                    var list = new List<MceTbUsuario>();

                    if (filterByStatus)
                        list = await db.MceTbUsuarios.Where(e => e.UsuStatus.Equals(filterByStatus)).ToListAsync();
                    else
                        list = await db.MceTbUsuarios.ToListAsync();

                    oResponse.Success = 1;
                    oResponse.Data = list;
                }
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
                using (DbCorreosInstUpiicsaContext db = new())
                {
                    var list = await db.MceTbUsuarios.FindAsync(id);
                    oResponse.Success = 1;
                    oResponse.Data = list;
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
                using (DbCorreosInstUpiicsaContext db = new())
                {
                    MceTbUsuario oUsuario = new()
                    {
                        IdUsuarioSolicitante = model.IdUsuarioSolicitante,
                        UsuIdTipoPersonal= model.UsuIdTipoPersonal,
                        UsuNombre = model.UsuNombre,
                        UsuPrimerApellido = model.UsuPrimerApellido,
                        UsuSegundoApellido = model.UsuSegundoApellido,
                        UsuCurp = model.UsuCurp,
                        UsuFilenameCurp = model.UsuFilenameCurp,
                        UsuArchivoCompInscripcion= model.UsuArchivoCompInscripcion,
                        UsuNoCelularAnterior = null,
                        UsuNoCelularNuevo = model.UsuNoCelularNuevo,
                        UsuIdCarrera= model.UsuIdCarrera,   
                        UsuBoletaAlumno = model.UsuBoletaAlumno,
                        UsuBoletaMaestria= model.UsuBoletaMaestria,
                        UsuSemestre=model.UsuSemestre,
                        UsuAñoEgreso= model.UsuAñoEgreso,
                        UsuNumeroEmpleado = model.UsuNumeroEmpleado,
                        UsuIdAreaDepto= model.UsuIdAreaDepto,
                        UsuExtension=model.UsuExtension,
                        UsuIdRol=model.UsuIdRol,
                        UsuCorreoPersonalAnterior=model.UsuCorreoPersonalAnterior,
                        UsuCorreoPersonalNuevo = model.UsuCorreoPersonalNuevo,
                        UsuContraseña = Encrypt.GetSHA256(model.UsuContraseña),
                        UsuRecuperarContraseñas = model.UsuRecuperarContraseñas,                                               
                        UsuCorreroInstitucional = model.UsuCorreroInstitucional,
                        UsuContraseñaInstitucional = model.UsuContraseñaInstitucional,                        
                        UsuFechaHoraAlta = model.UsuFechaHoraAlta,
                        UsuStatus = model.UsuStatus,                                                
                        //UsuIdTipoPersonalNavigation = null
                    };
                    await db.MceTbUsuarios.AddAsync(oUsuario);
                    await db.SaveChangesAsync();

                    oResponse.Success = 1;
                }
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
                MceTbUsuario? oUsuario = db.MceTbUsuarios.Find(model.IdUsuarioSolicitante);
                if (oUsuario != null)
                {
                    oUsuario.UsuNombre = model.UsuNombre;
                    oUsuario.UsuPrimerApellido = model.UsuPrimerApellido;
                    oUsuario.UsuSegundoApellido = model.UsuSegundoApellido;
                    oUsuario.UsuCurp = model.UsuCurp;
                    oUsuario.UsuFilenameCurp = model.UsuFilenameCurp;
                    oUsuario.UsuNoCelularAnterior = null;
                    oUsuario.UsuNoCelularNuevo = model.UsuNoCelularNuevo;
                    oUsuario.UsuBoletaAlumno = model.UsuBoletaAlumno;
                    oUsuario.UsuNumeroEmpleado = model.UsuNumeroEmpleado;
                    oUsuario.UsuCorreoPersonalNuevo = model.UsuCorreoPersonalNuevo;
                    oUsuario.UsuContraseña = model.UsuContraseña;
                    oUsuario.UsuRecuperarContraseñas = model.UsuRecuperarContraseñas;
                    oUsuario.UsuIdTipoPersonal = model.UsuIdTipoPersonal;
                    oUsuario.UsuIdRol = model.UsuIdRol;
                    oUsuario.UsuCorreroInstitucional = model.UsuCorreroInstitucional;
                    oUsuario.UsuContraseñaInstitucional = model.UsuContraseñaInstitucional;
                    oUsuario.UsuIdCarrera = model.UsuIdCarrera;
                    oUsuario.UsuFechaHoraAlta = model.UsuFechaHoraAlta;
                    oUsuario.UsuStatus = model.UsuStatus;

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
                MceTbUsuario? oUsuario = db.MceTbUsuarios.Find(id);
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
