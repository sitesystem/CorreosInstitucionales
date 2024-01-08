using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers.MóduloSolicitudes
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class SolicitudesController(DbCorreosInstitucionalesUpiicsaContext db) : ControllerBase
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;

        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllData(int filterByStatus)
        {
            Response<List<MtTbSolicitudesTicket>> oResponse = new();

            try
            {
                var list = new List<MtTbSolicitudesTicket>();

                if (filterByStatus > 0)
                    list = await _db.MtTbSolicitudesTickets.Where(st => st.SolIdEstadoSolicitud.Equals(filterByStatus)).ToListAsync();
                                    //   .OrderByDescending(x => x.Id)
                                    //   .Skip((actualPage - 1) * Utilities.REGISTERSPERPAGE)
                                    //   .Take(Utilities.REGISTERSPERPAGE)
                                    //   .ToList();
                else
                    list = await _db.MtTbSolicitudesTickets.ToListAsync();

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
            Response<MtTbSolicitudesTicket> oResponse = new();

            try
            {
                var list = await _db.MtTbSolicitudesTickets.FindAsync(id);
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
            Response<List<MtTbSolicitudesTicket>> oResponse = new();

            try
            {
                var list = new List<MtTbSolicitudesTicket>();

                int solicitudAtendida = await _db.MtTbSolicitudesTickets
                                                 .Where(st => st.IdSolicitudTicket.Equals(filterById) && st.SolIdEstadoSolicitud.Equals(filterByStatus))
                                                 .CountAsync();
                if (solicitudAtendida > 0)
                    oResponse.Message = "NO PUEDE SOLICITAR";
                else
                    oResponse.Message = "SI PUEDE SOLICITAR";

                if (filterByStatus > 0)
                    list = await _db.MtTbSolicitudesTickets
                                    .Where(st => st.IdSolicitudTicket.Equals(filterById) && st.SolIdEstadoSolicitud.Equals(filterByStatus))
                                    .ToListAsync();
                else
                    list = await _db.MtTbSolicitudesTickets
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
        public async Task<IActionResult> AddData(RequestDTO_Solicitud model)
        {
            Response<object> oResponse = new();

            try
            {
                MtTbSolicitudesTicket oSolicitud = new()
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

                await _db.MtTbSolicitudesTickets.AddAsync(oSolicitud);
                await _db.SaveChangesAsync();

                oResponse.Success = 1;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpPut]
        public async Task<IActionResult> EditData(RequestDTO_Solicitud model)
        {
            Response<object> oRespuesta = new();

            try
            {
                MtTbSolicitudesTicket? oSolicitud = _db.MtTbSolicitudesTickets.Find(model.IdSolicitudTicket);

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

                    _db.Entry(oSolicitud).State = EntityState.Modified;
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

        [HttpPut("editByIdStatus/{id}/{status}")]
        public async Task<IActionResult> EditStatusById(int id, int status)
        {
            Response<object> oRespuesta = new();

            try
            {
                MtTbSolicitudesTicket? oSolicitud = await _db.MtTbSolicitudesTickets.FindAsync(id);
                //db.Remove(oPersona);

                if (oSolicitud != null)
                {
                    oSolicitud.SolIdEstadoSolicitud = status;
                    _db.Entry(oSolicitud).State = EntityState.Modified;
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
