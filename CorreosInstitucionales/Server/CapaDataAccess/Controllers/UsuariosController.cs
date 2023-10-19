using CorreosInstitucionales.Server.CapaDataAccess.DBContext;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
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
        public async Task <IActionResult> GetDataById(int id)
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
        public async Task <IActionResult> AddData(UsuarioViewModel model)
        {
            Response<object> oResponse = new();

            try
            {
                using (DbCorreosInstUpiicsaContext db = new())
                {
                    MceTbUsuario oUsuario = new()
                    {
                        IdUsuarioSolicitante=model.IdUsuarioSolicitante,
                        UsuNombre=model.UsuNombre,
                        UsuPrimerApellido=model.UsuPrimerApellido,
                        UsuSegundoApellido=model.UsuSegundoApellido,
                        UsuBoleta=model.UsuBoleta,
                        UsuNumeroEmpleado=model.UsuNumeroEmpleado,
                        UsuContraseña=model.UsuContraseña,
                        UsuRecuperarContraseñas=model.UsuRecuperarContraseñas,
                        UsuIdTipoPersonal=model.UsuIdTipoPersonal,
                        UsuStatus=model.UsuStatus,
                        UsuCorreoPersonal=model.UsuCorreoPersonal,
                        UsuIdRol=model.UsuIdRol,
                        UsuCorreroInstitucional=model.UsuCorreroInstitucional,
                        UsuContraseñaInstitucional=model.UsuContraseñaInstitucional,
                        UsuIdCarrera=model.UsuIdCarrera,
                        //UsuIdTipoPersonalNavigation=null
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
        public IActionResult EditData(UsuarioViewModel model)
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
                    oUsuario.UsuBoleta = model.UsuBoleta;
                    oUsuario.UsuNumeroEmpleado = model.UsuNumeroEmpleado;
                    oUsuario.UsuContraseña = model.UsuContraseña;
                    oUsuario.UsuRecuperarContraseñas = model.UsuRecuperarContraseñas;
                    oUsuario.UsuIdTipoPersonal=model.UsuIdTipoPersonal;
                    oUsuario.UsuStatus = model.UsuStatus;
                    oUsuario.UsuCorreoPersonal = model.UsuCorreoPersonal;
                    oUsuario.UsuIdRol = model.UsuIdRol;
                    oUsuario.UsuCorreroInstitucional = model.UsuCorreroInstitucional;
                    oUsuario.UsuContraseñaInstitucional = model.UsuContraseñaInstitucional;
                    oUsuario.UsuIdCarrera = model.UsuIdCarrera;

                    db.Entry(oUsuario).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
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
        [HttpPut("editByIdStatus/{Id}/{isActivate}")]
        public IActionResult EnableDisableDataById(int id, bool isActivate)
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
                    db.Entry(oUsuario).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
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
