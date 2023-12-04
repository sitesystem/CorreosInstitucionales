using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using CorreosInstitucionales.Server.CapaDataAccess.DBContext;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class SolicitudesController : ControllerBase
    {
        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllData(int filterByStatus)
        {
            Response<List<MceTbSolicitudTicket>> oResponse = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();
                var list = new List<MceTbSolicitudTicket>();

                if (filterByStatus > 0)
                    list = await db.MceTbSolicitudTickets.Where(st => st.SolIdEstadoSolicitud.Equals(filterByStatus)).ToListAsync();
                                                    //   .OrderByDescending(x => x.Id)
                                                    //   .Skip((actualPage - 1) * Utilities.REGISTERSPERPAGE)
                                                    //   .Take(Utilities.REGISTERSPERPAGE)
                                                    //   .ToList();
                else
                    list = await db.MceTbSolicitudTickets.ToListAsync();

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
            Response<MceTbSolicitudTicket> oResponse = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();
                var list = await db.MceTbSolicitudTickets.FindAsync(id);
                oResponse.Success = 1;
                oResponse.Data = list;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpGet("filterByIdStatus/{filterById}/{filterByStatus}")]
        public async Task<IActionResult> GetDataByIdStatus(int filterById, int filterByStatus)
        {
            Response<List<MceTbSolicitudTicket>> oResponse = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();
                var list = new List<MceTbSolicitudTicket>();

                int solicitudAtendida = await db.MceTbSolicitudTickets
                                              .Where(st => st.IdSolicitudTicket.Equals(filterById) && st.SolIdEstadoSolicitud.Equals(filterByStatus))
                                              .CountAsync();
                if (solicitudAtendida > 0)
                    oResponse.Message = "NO PUEDE SOLICITAR";
                else
                    oResponse.Message = "SI PUEDE SOLICITAR";

                if (filterByStatus > 0)
                    list = await db.MceTbSolicitudTickets
                                 .Where(st => st.IdSolicitudTicket.Equals(filterById) && st.SolIdEstadoSolicitud.Equals(filterByStatus))
                                 //.Where(st => st.SolIdEstadoSolicitud.Equals(filterByStatus))
                                 .ToListAsync();
                else
                    list = await db.MceTbSolicitudTickets
                                 .Where(st => st.IdSolicitudTicket.Equals(filterById))
                                 .ToListAsync();

                oResponse.Success = 1;
                oResponse.Data = list;
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
                using DbCorreosInstUpiicsaContext db = new();

                MceTbSolicitudTicket oSolicitud = new()
                {
                    IdSolicitudTicket = model.IdSolicitudTicket,
                    SolIdTipoSolicitud = model.SolIdTipoSolicitud,
                    SolIdUsuario = model.SolIdUsuario,
                    SolCapturaEscaneoAntivirus = model.SolCapturaEscaneoAntivirus,
                    SolCapturaCuentaBloqueada = model.SolCapturaCuentaBloqueada,
                    SolCapturaError = model.SolCapturaError,
                    SolFechaHoraCreacion = DateTime.UtcNow,
                    SolIdEstadoSolicitud = 1,
                    SolIdEstadoSolicitudNavigation = null,
                    SolIdTipoSolicitudNavigation = null,
                    SolIdUsuarioNavigation = null
                };

                await db.MceTbSolicitudTickets.AddAsync(oSolicitud);
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
        public async Task<IActionResult> EditData(SolicitudViewModel model)
        {
            Response<object> oRespuesta = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();

                MceTbSolicitudTicket? oSolicitud = db.MceTbSolicitudTickets.Find(model.IdSolicitudTicket);

                if (oSolicitud != null)
                {
                    oSolicitud.SolIdTipoSolicitud = model.SolIdTipoSolicitud;
                    oSolicitud.SolIdUsuario = model.SolIdUsuario;
                    oSolicitud.SolCapturaEscaneoAntivirus = model.SolCapturaEscaneoAntivirus;
                    oSolicitud.SolCapturaCuentaBloqueada = model.SolCapturaCuentaBloqueada;
                    oSolicitud.SolCapturaError = model.SolCapturaError;
                    //oSolicitud.SolFechaHoraCreacion = model.SolFechaHoraCreacion;
                    //oSolicitud.SolIdEstadoSolicitud = model.SolIdEstadoSolicitud;
                    oSolicitud.SolIdEstadoSolicitudNavigation = null;
                    oSolicitud.SolIdTipoSolicitudNavigation = null;
                    oSolicitud.SolIdUsuarioNavigation = null;

                    db.Entry(oSolicitud).State = EntityState.Modified;
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

        [HttpPut("editByIdStatus/{id}/{status}")]
        public async Task<IActionResult> EditStatusById(int id, int status)
        {
            Response<object> oRespuesta = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();

                MceTbSolicitudTicket? oSolicitud = await db.MceTbSolicitudTickets.FindAsync(id);
                //db.Remove(oPersona);

                if (oSolicitud != null)
                {
                    oSolicitud.SolIdEstadoSolicitud = status;
                    db.Entry(oSolicitud).State = EntityState.Modified;
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
