using CorreosInstitucionales.Server.CapaDataAccess.DBContext;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudesController : ControllerBase
    {
        [HttpGet("filterById/{id}")]
        public async Task<IActionResult> GetDataById(int id)
        {
            Response<MceTbSolicitud> oResponse = new();

            try
            {
                using (DbCorreosInstUpiicsaContext db = new())
                {
                    var list = await db.MceTbSolicituds.FindAsync(id);
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
        public async Task<IActionResult> AddData(SolicitudViewModel model)
        {
            Response<object> oResponse = new();

            try
            {
                using (DbCorreosInstUpiicsaContext db = new())
                {
                    MceTbSolicitud oSolicitud= new()
                    {
                        IdSolicitud=model.IdSolicitud,
                        SolIdTipoSolicitud=model.SolIdTipoSolicitud,
                        SolIdEstadosSolicitud=model.SolIdEstadosSolicitud,
                        SolIdAreaDepto=model.SolIdAreaDepto,
                        SolIdUsuario=model.SolIdUsuario,
                        SolFecha=model.SolFecha,
                    };
                    await db.MceTbSolicituds.AddAsync(oSolicitud);
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
        public IActionResult EditData(SolicitudViewModel model)
        {
            Response<object> oRespuesta = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();
                MceTbSolicitud? oSolicitud = db.MceTbSolicituds.Find(model.IdSolicitud);
                if (oSolicitud != null)
                {
                    oSolicitud.IdSolicitud = model.IdSolicitud;
                    oSolicitud.SolIdTipoSolicitud = model.SolIdTipoSolicitud;
                    oSolicitud.SolIdEstadosSolicitud = model.SolIdEstadosSolicitud;
                    oSolicitud.SolIdAreaDepto = model.SolIdAreaDepto;
                    oSolicitud.SolIdUsuario = model.SolIdUsuario;
                    oSolicitud.SolFecha = model.SolFecha;
                    ;

                    db.Entry(oSolicitud).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
