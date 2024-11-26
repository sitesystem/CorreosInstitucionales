using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

using CorreosInstitucionales.Server.CapaDataAccess.Controllers.SendEmail;
using CorreosInstitucionales.Server.Utils;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolSendWhatsApp;
using CorreosInstitucionales.Shared.Constantes;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolNotificaciones;
using CorreosInstitucionales.Shared.CapaTools;
using Serilog;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers.MóduloSolicitudes
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class SolicitudesController
        (
            DbCorreosInstitucionalesUpiicsaContext db,
            RSendNotificacionesService servicioNotificaciones
        ) : ControllerBase
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;
        private readonly RSendNotificacionesService _servicioNotificaciones = servicioNotificaciones;

        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllDataByStatus(bool filterByStatus)
        {
            Response<List<MtTbSolicitudesTicket>> oResponse = new();

            try
            {
                var list = new List<MtTbSolicitudesTicket>();

                if (filterByStatus)
                    list = await _db.MtTbSolicitudesTickets
                                    .Where(st => st.SolIdEstadoSolicitud != 6)
                                    .Include(u => u.SolIdUsuarioNavigation)
                                        .ThenInclude(tp => tp!.UsuIdTipoPersonalNavigation)
                                    .Include(st => st.SolIdEstadoSolicitudNavigation)
                                    .Include(st => st.SolIdTipoSolicitud)
                                    .ToListAsync();
                else
                    list = await _db.MtTbSolicitudesTickets
                                    .Include(u => u.SolIdUsuarioNavigation)
                                        .ThenInclude(tp => tp!.UsuIdTipoPersonalNavigation)
                                    .Include(st => st.SolIdEstadoSolicitudNavigation)
                                    .Include(st => st.SolIdTipoSolicitudNavigation)
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

        [HttpPost("filterByProgress")]
        public async Task<IActionResult> GetAllDataByProgress(int[] progress)
        {
            Response<List<MtTbSolicitudesTicket>> oResponse = new();

            try
            {
                var list = await _db.MtTbSolicitudesTickets
                            .Where(
                                st => progress.Contains(st.SolIdEstadoSolicitud)
                            )
                            .Include(st => st.SolIdUsuarioNavigation)
                                .ThenInclude(tp => tp!.UsuIdTipoPersonalNavigation)
                            .Include(u => u.SolIdUsuarioNavigation)
                                .ThenInclude(c => c!.UsuIdCarreraNavigation)
                            .Include(u => u.SolIdUsuarioNavigation)
                                .ThenInclude(a => a!.UsuIdAreaDeptoNavigation)
                            .Include(st => st.SolIdEstadoSolicitudNavigation)
                            .Include(st => st.SolIdTipoSolicitudNavigation)
                            .ToListAsync();

                oResponse.Data = list;
                oResponse.Message = list.Count.ToString();
                oResponse.Success = 1;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpGet("filterByIdUsuario/{filterByIdUsuario}")]
        public async Task<IActionResult> GetAllDataByIdUsuario(int filterByIdUsuario)
        {
            Response<List<MtTbSolicitudesTicket>> oResponse = new();

            try
            {
                var list = new List<MtTbSolicitudesTicket>();

                list = await _db.MtTbSolicitudesTickets
                                .Where(st => st.SolIdUsuario.Equals(filterByIdUsuario))
                                .Include(ts => ts.SolIdTipoSolicitudNavigation)
                                .Include(u => u.SolIdUsuarioNavigation)
                                .Include(e => e.SolIdEstadoSolicitudNavigation)
                                .OrderByDescending(st => st.IdSolicitudTicket)
                                .ToListAsync();

                oResponse.Success = 1;
                oResponse.Message = list.Count.ToString();
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
                int solicitudNoAtendida = await _db.MtTbSolicitudesTickets
                                                   .Where(st => st.SolIdUsuario.Equals(filterByIdUsuarioStatus) &&
                                                                !st.SolIdEstadoSolicitud.Equals(5) && !st.SolIdEstadoSolicitud.Equals(6))
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

        [HttpGet("filterByIdUsuarioLastTicket/{filterByIdUsuarioLastTicket}")]
        public async Task<IActionResult> GetDataByIdUsuarioLastTicket(int filterByIdUsuarioLastTicket)
        {
            Response<MtTbSolicitudesTicket> oResponse = new();

            try
            {
                var item = await _db.MtTbSolicitudesTickets
                                    .Where(st => st.SolIdUsuario.Equals(filterByIdUsuarioLastTicket) &&
                                                 st.SolIdEstadoSolicitud != 5 && st.SolIdEstadoSolicitud != 6)
                                    .Include(ts => ts.SolIdTipoSolicitudNavigation)
                                    .Include(u => u.SolIdUsuarioNavigation)
                                    .Include(e => e.SolIdEstadoSolicitudNavigation)
                                    .OrderByDescending(st => st.IdSolicitudTicket)
                                    .FirstOrDefaultAsync();

                oResponse.Success = 1;
                oResponse.Data = item;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpGet("filterByIdUsuarioLastTicketRespuesta/{filterByIdUsuarioLastTicketRespuesta}")]
        public async Task<IActionResult> GetDataByIdUsuarioLastTicketRespuesta(int filterByIdUsuarioLastTicketRespuesta)
        {
            Response<MtTbSolicitudesTicket> oResponse = new();

            try
            {
                var item = await _db.MtTbSolicitudesTickets
                                    .Where(st => st.SolIdUsuario.Equals(filterByIdUsuarioLastTicketRespuesta))
                                    .Include(ts => ts.SolIdTipoSolicitudNavigation)
                                    .Include(u => u.SolIdUsuarioNavigation)
                                    .Include(e => e.SolIdEstadoSolicitudNavigation)
                                    .OrderByDescending(st => st.IdSolicitudTicket)
                                    .FirstOrDefaultAsync();

                oResponse.Success = 1;
                oResponse.Data = item;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpPost("countDataByIdUserProgress/{idUser}")]
        public async Task<IActionResult> GetCountDataByProgress(int idUser, int[] progress)
        {
            Response<object> oResponse = new();

            try
            {
                var count = 0;

                if (idUser == 0)
                    count = await _db.MtTbSolicitudesTickets.Where(st => progress.Contains(st.SolIdEstadoSolicitud)).CountAsync();
                else
                    count = await _db.MtTbSolicitudesTickets.Where(st => st.SolIdUsuario.Equals(idUser)).CountAsync();

                oResponse.Message = count.ToString();
                oResponse.Success = 1;
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
                MpTbUsuario? oUsuario = await _db.MpTbUsuarios.FindAsync(model.SolIdUsuario);

                if (oUsuario != null)
                {
                    oUsuario.UsuFileNameCurp = model.SolFileNameCurp;
                    oUsuario.UsuFileNameComprobanteEstudios = model.SolFileNameComprobanteInscripcion;

                    if (model.SolIdTipoSolicitud == 2)
                    {
                        oUsuario.UsuCorreoPersonalCuentaAnterior = model.SolCorreoPersonalCuentaAnterior?.Trim();
                        oUsuario.UsuCorreoPersonalCuentaActual = model.SolCorreoPersonalCuentaActual!.Trim();
                    }
                    else if (model.SolIdTipoSolicitud == 3)
                    {
                        oUsuario.UsuNoCelularAnterior = model.SolNoCelularAnterior;
                        oUsuario.UsuNoCelularActual = model.SolNoCelularActual!;
                    }

                    _db.Entry(oUsuario).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                }

                MtTbSolicitudesTicket oSolicitud = new()
                {
                    IdSolicitudTicket = model.IdSolicitudTicket,
                    SolToken = model.SolToken,
                    SolIdTipoSolicitud = model.SolIdTipoSolicitud,
                    SolIdUsuario = model.SolIdUsuario,
                    SolCapturaEscaneoAntivirus = model.SolCapturaEscaneoAntivirus,
                    SolCapturaCuentaBloqueada = model.SolCapturaCuentaBloqueada,
                    SolCapturaError = model.SolCapturaError,
                    SolObservacionesSolicitud = model.SolObservacionesSolicitud?.Trim(),
                    SolIdEstadoSolicitud = model.SolIdEstadoSolicitud,
                    SolFechaHoraActualizacion = DateTime.Now,
                    SolValidacionDatos = model.SolValidacionDatos,
                    SolEnvioEncuesta = model.SolEnvioEncuesta,
                    SolEncuestaCalidadCalificacion = model.SolEncuestaCalidadCalificacion,
                    SolEncuestaCalidadComentarios = model.SolEncuestaCalidadComentarios,
                    SolFechaHoraEncuesta = model.SolFechaHoraEncuesta,
                    SolRespuestaDcyC = model.SolRespuestaDcyC,
                    SolFechaHoraCreacion = DateTime.Now,
                    // DATOS FK NAVIGATION
                    SolIdTipoSolicitudNavigation = null!,
                    SolIdUsuarioNavigation = null!,
                    SolIdEstadoSolicitudNavigation = null!,
                };

                await _db.MtTbSolicitudesTickets.AddAsync(oSolicitud);
                await _db.SaveChangesAsync();
                oResponse.Success = 1;

                oCreatedAtActionResult = CreatedAtAction(nameof(AddData), new { id = oSolicitud.IdSolicitudTicket }, oSolicitud);
                oResponse.Message = oSolicitud.IdSolicitudTicket.ToString(); // PK ID Único de la Solicitud-Ticket creado o dado de alta
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
                    // oSolicitud.SolToken = model.SolToken;
                    oSolicitud.SolIdTipoSolicitud = model.SolIdTipoSolicitud;
                    oSolicitud.SolIdUsuario = model.SolIdUsuario;
                    oSolicitud.SolCapturaEscaneoAntivirus = model.SolCapturaEscaneoAntivirus;
                    oSolicitud.SolCapturaCuentaBloqueada = model.SolCapturaCuentaBloqueada;
                    oSolicitud.SolCapturaError = model.SolCapturaError;
                    oSolicitud.SolObservacionesSolicitud = model.SolObservacionesSolicitud;
                    oSolicitud.SolIdEstadoSolicitud = model.SolIdEstadoSolicitud;
                    oSolicitud.SolFechaHoraActualizacion = DateTime.Now;
                    oSolicitud.SolValidacionDatos = model.SolValidacionDatos;
                    oSolicitud.SolEnvioEncuesta = model.SolEnvioEncuesta;
                    oSolicitud.SolEncuestaCalidadCalificacion = model.SolEncuestaCalidadCalificacion;
                    oSolicitud.SolEncuestaCalidadComentarios = model.SolEncuestaCalidadComentarios;
                    oSolicitud.SolFechaHoraEncuesta = model.SolFechaHoraEncuesta;
                    oSolicitud.SolRespuestaDcyC = model.SolRespuestaDcyC;
                    // oSolicitud.SolFechaHoraCreacion = DateTime.Now;
                    oSolicitud.SolIdTipoSolicitudNavigation = null!;
                    oSolicitud.SolIdUsuarioNavigation = null!;
                    oSolicitud.SolIdEstadoSolicitudNavigation = null!;

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
                // db.Remove(oPersona);

                if (oSolicitud != null)
                {
                    oSolicitud.SolIdEstadoSolicitud = status;
                    oSolicitud.SolFechaHoraActualizacion = DateTime.Now;

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

                if (oSolicitud != null)
                {
                    oSolicitud.SolIdEstadoSolicitud = 5;
                    //oSolicitud.SolFechaHoraActualizacion = DateTime.Now; // No se actualiza la Fecha Hora de Encuesta Contestada ya tiene su campo SolFechaHoraEncuesta
                    oSolicitud.SolEncuestaCalidadCalificacion = model.Calificacion;
                    oSolicitud.SolEncuestaCalidadComentarios = model.Comentarios.Trim();
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
        } // ENCUESTA DE CALIDAD

        [HttpGet("generar_random")]
        public async Task<IActionResult> GenerarSolicitudes()
        {
            Response<object> oRespuesta = new();
            MtTbSolicitudesTicket oSolicitud;
            TipoDocumento[] documentos = [];

            int[] terminados =
            [
                (int)TipoEstadoSolicitud.ATENDIDA,
                (int)TipoEstadoSolicitud.ENCUESTA_CALIDAD,
                (int)TipoEstadoSolicitud.CANCELADA
            ];

            try
            {
                List<int> tipo_solicitudes = await _db.McCatTiposSolicituds
                    .Select(ts => ts.IdTipoSolicitud)
                    .ToListAsync();

                //USUARIOS QUE YA TIENEN UNA SOLICITUD PENDIENTE
                List<int> usuarios = await _db.MtTbSolicitudesTickets
                    .Where(st => !terminados.Contains(st.SolIdEstadoSolicitud))
                    .Select(st => st.SolIdUsuario)
                    .Distinct()
                    .ToListAsync();

                List<MpTbUsuario> pendientes = await _db.MpTbUsuarios
                    .Where(u => !usuarios.Contains(u.IdUsuario))
                    .ToListAsync();

                int tipo_solicitud;
                Random rnd = new();
                string uid = string.Empty;
                int estado_pendiente = (int)TipoEstadoSolicitud.PENDIENTE;

                foreach (MpTbUsuario pendiente in pendientes)
                {
                    uid = Guid.NewGuid().ToString();
                    tipo_solicitud = rnd.Next(1, 7);

                    oSolicitud = new()
                    {
                        SolToken = uid,
                        SolIdTipoSolicitud = tipo_solicitud,
                        SolIdUsuario = pendiente.IdUsuario,

                        SolObservacionesSolicitud = null,
                        SolIdEstadoSolicitud = estado_pendiente,
                        SolValidacionDatos = false,
                        SolEncuestaCalidadCalificacion = null,
                        SolEncuestaCalidadComentarios = null,
                        SolFechaHoraEncuesta = null,
                        SolRespuestaDcyC = null,
                        SolFechaHoraCreacion = DateTime.Now
                    };

                    documentos = ((TipoSolicitud)tipo_solicitud).GetDocumentos();

                    oSolicitud.SolCapturaEscaneoAntivirus = documentos.Contains(TipoDocumento.CAP_ANTIVIRUS) ? $"CAPTURA_ANTIVIRUS_{uid}.pdf" : "-";
                    oSolicitud.SolCapturaCuentaBloqueada = documentos.Contains(TipoDocumento.CAP_BLOQUEO) ? $"CAPTURA_BLOQUEO_{uid}.pdf" : "-";
                    oSolicitud.SolCapturaError = documentos.Contains(TipoDocumento.CAP_ERROR) ? $"CAPTURA_ERROR_{uid}.pdf" : "-";

                    await _db.MtTbSolicitudesTickets.AddAsync(oSolicitud);
                }

                await _db.SaveChangesAsync();
                oRespuesta.Success = 1;

            }
            catch (Exception ex)
            {
                oRespuesta.Message = ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return Ok(oRespuesta);
        } // GENERAR SOLICITUDES

        [HttpPatch("finalizar")]
        public async Task<IActionResult> FinalizarSolicitud(RequestDTO_FinalizarSolicitud oFinalizarSolicitud) // KeyValuePair<int, string> datos)
        {
            Response<string> oRespuesta = new();
            int guardados = 0;

            MtTbSolicitudesTicket? oSolicitud = null;
            
            try
            {
                oSolicitud = await _db.MtTbSolicitudesTickets!
                                      .Where(st => st.IdSolicitudTicket == oFinalizarSolicitud.IdSolicitud)
                                      .Include(u => u.SolIdUsuarioNavigation)
                                      .FirstAsync();

                //FindAsync(oFinalizarSolicitud.IdSolicitud); // (datos.Key)

                if (oSolicitud is not null)
                {
                    oSolicitud.SolIdEstadoSolicitud = oFinalizarSolicitud.Estado;
                    oSolicitud.SolRespuestaDcyC = oFinalizarSolicitud.Mensaje; // datos.Value;
                    oSolicitud.SolFechaHoraActualizacion = DateTime.Now;

                    _db.Entry(oSolicitud).State = EntityState.Modified;
                    guardados = await _db.SaveChangesAsync();
                }

                if (guardados == 1)
                {
                    oRespuesta.Success = 1;

                    Dictionary<string, object?> datos = new()
                    {
                        {"solicitud", oSolicitud },
                        {"usuario", oSolicitud!.SolIdUsuarioNavigation },
                    };

                    int filtro = (TipoEstadoSolicitud)oSolicitud.SolIdEstadoSolicitud != TipoEstadoSolicitud.ATENDIDA ||
                        ((TipoPersonal)oSolicitud.SolIdUsuarioNavigation!.UsuIdTipoPersonal).EsAlumnoOEgresado() ? 0 : 1;

                    Response<Notificacion?> notificacion = PlantillaManager.GetNotificacion(datos, oFinalizarSolicitud.Estado, filtro);

                    if(notificacion.Success == 1)
                    {
                        Response<string> response = await _servicioNotificaciones.EnviarAsync(notificacion.Data!);

                        if(response.Success == 1)
                        {
                            oRespuesta.Success = 1;
                            oRespuesta.Data = "";
                        }
                        else
                        {
                            oRespuesta.Message = response.Data??"???";
                        }
                    }
                    else
                    {
                        oRespuesta.Message = $"[ERROR] NO SE PUDO GENERAR LA NOTIFICACIÓN:\n{notificacion.Message}";
                    }
                }
            }
            catch (Exception ex)
            {
                oRespuesta.Message = ex.Message;
            }

            return Ok(oRespuesta);
        } // CANCELAR
    }
}
