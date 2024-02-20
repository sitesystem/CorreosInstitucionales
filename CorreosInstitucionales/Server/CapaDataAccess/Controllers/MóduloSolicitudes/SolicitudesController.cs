using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using CorreosInstitucionales.Client.CapaPresentation_ComponentsPages_UI_UX.MóduloCatálogos;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers.MóduloSolicitudes
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class SolicitudesController(DbCorreosInstitucionalesUpiicsaContext db) : ControllerBase
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;

        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllDataByStatus(bool filterByStatus)
        {
            Response<List<MtTbSolicitudesTicket>> oResponse = new();

            try
            {
                var list = new List<MtTbSolicitudesTicket>();

                if (filterByStatus)
                    list = await _db.MtTbSolicitudesTickets
                                    .Where(st => !st.SolIdEstadoSolicitud.Equals(4)).ToListAsync();
                                    // .OrderByDescending(x => x.Id)
                                    // .Skip((actualPage - 1) * Utilities.REGISTERSPERPAGE)
                                    // .Take(Utilities.REGISTERSPERPAGE)
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
                var item = await _db.MtTbSolicitudesTickets.FindAsync(id);
                oResponse.Success = 1;
                oResponse.Data = item;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpGet("filterByIdUsuarioStatus/{filterByIdUsuarioStatus}")]
        public async Task<IActionResult> GetDataByIdUsuarioStatus(int filterByIdUsuarioStatus)
        {
            Response<MtTbSolicitudesTicket> oResponse = new();

            try
            {
                var list = new List<MtTbSolicitudesTicket>();

                int solicitudNoAtendida = await _db.MtTbSolicitudesTickets
                                                   .Where(st => st.SolIdUsuario.Equals(filterByIdUsuarioStatus) && (!st.SolIdEstadoSolicitud.Equals(5) || !st.SolIdEstadoSolicitud.Equals(6)))
                                                   .CountAsync();
                if (solicitudNoAtendida > 0)
                    oResponse.Message = "NO PUEDE SOLICITAR, PENDIENTE DE CONTESTAR ENCUESTA DE CALIDAD";
                else
                {
                    oResponse.Success = 1;
                    oResponse.Message = "SI PUEDE SOLICITAR";
                }

                oResponse.Data = null;
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
            CreatedAtActionResult oCreatedAtActionResult;

            try
            {
                MtTbSolicitudesTicket oSolicitud = new()
                {
                    IdSolicitudTicket = model.IdSolicitudTicket,
                    SolToken = model.SolToken,
                    SolIdTipoSolicitud = model.SolIdTipoSolicitud,
                    SolIdUsuario = model.SolIdUsuario,
                    SolCapturaEscaneoAntivirus = model.SolCapturaEscaneoAntivirus,
                    SolCapturaCuentaBloqueada = model.SolCapturaCuentaBloqueada,
                    SolCapturaError = model.SolCapturaError,
                    SolObservacionesSolicitud = model.SolObservacionesSolicitud,
                    SolIdEstadoSolicitud = model.SolIdEstadoSolicitud,
                    SolValidacionDatos = model.SolValidacionDatos,
                    SolEncuestaCalidadCalificacion = model.SolEncuestaCalidadCalificacion,
                    SolEncuestaCalidadComentarios = model.SolEncuestaCalidadComentarios,
                    SolFechaHoraEncuesta = model.SolFechaHoraEncuesta,
                    SolFechaHoraCreacion = DateTime.Now,
                    SolIdTipoSolicitudNavigation = null,
                    SolIdUsuarioNavigation = null,
                    SolIdEstadoSolicitudNavigation = null,
                };

                await _db.MtTbSolicitudesTickets.AddAsync(oSolicitud);
                await _db.SaveChangesAsync();
                oResponse.Success = 1;

                oCreatedAtActionResult = CreatedAtAction(nameof(AddData), new { id = oSolicitud.IdSolicitudTicket }, oSolicitud);
                oResponse.Message = oSolicitud.IdSolicitudTicket.ToString(); // PK ID Único de la Solicitud-Ticket Creado o dado de Alta
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
                    oSolicitud.SolToken = model.SolToken;
                    oSolicitud.SolIdTipoSolicitud = model.SolIdTipoSolicitud;
                    oSolicitud.SolIdUsuario = model.SolIdUsuario;
                    oSolicitud.SolCapturaEscaneoAntivirus = model.SolCapturaEscaneoAntivirus;
                    oSolicitud.SolCapturaCuentaBloqueada = model.SolCapturaCuentaBloqueada;
                    oSolicitud.SolCapturaError = model.SolCapturaError;
                    oSolicitud.SolObservacionesSolicitud = model.SolObservacionesSolicitud;
                    oSolicitud.SolIdEstadoSolicitud = model.SolIdEstadoSolicitud;
                    oSolicitud.SolValidacionDatos = model.SolValidacionDatos;
                    oSolicitud.SolEncuestaCalidadCalificacion = model.SolEncuestaCalidadCalificacion;
                    oSolicitud.SolEncuestaCalidadComentarios = model.SolEncuestaCalidadComentarios;
                    oSolicitud.SolFechaHoraEncuesta = model.SolFechaHoraEncuesta;
                    // oSolicitud.SolFechaHoraCreacion = model.SolFechaHoraCreacion;
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

        [HttpPut("editStatusById/{id}/{status}")]
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

        [HttpPut("encuesta/calidad")]
        public async Task<IActionResult> EncuestaCalidad(RequestDTO_EncuestaCalidad model)
        {
            Response<object> oRespuesta = new();

            try
            {
                MtTbSolicitudesTicket? oSolicitud = await _db.MtTbSolicitudesTickets.FindAsync(model.IdSolicitud);
                //db.Remove(oPersona);

                if (oSolicitud != null)
                {
                    oSolicitud.SolEncuestaCalidadCalificacion = model.Calificacion;
                    oSolicitud.SolEncuestaCalidadComentarios = model.Comentarios;
                    oSolicitud.SolFechaHoraEncuesta = DateTime.Now;

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
