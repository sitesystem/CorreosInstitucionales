using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using CorreosInstitucionales.Shared.Constantes;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using CorreosInstitucionales.Server.CapaDataAccess.Controllers.SendEmail;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolSendWhatsApp;
using CorreosInstitucionales.Shared.CapaTools;
using CorreosInstitucionales.Server.Correos;
using CorreosInstitucionales.Server.MensajesWA;
using DocumentFormat.OpenXml.Wordprocessing;
using CorreosInstitucionales.Client.CapaPresentationComponentsPagesUI_UX.Debug;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers.MóduloSolicitudes
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class SolicitudesController
        (
            DbCorreosInstitucionalesUpiicsaContext db,
            ISendEmailService servicioEmail,
            IServiceProvider serviceProvider,
            ISendWhatsAppService servicioWA
        ) : ControllerBase
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;
        private readonly ISendEmailService _servicioCorreo = servicioEmail;
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ISendWhatsAppService _servicioWA = servicioWA;

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
                                    .Include(u=>u.SolIdUsuarioNavigation)
                                    .Include(st=>st.SolIdEstadoSolicitudNavigation)
                                    .Include(st=>st.SolIdTipoSolicitud)
                                    .ToListAsync();
                else
                    list = await _db.MtTbSolicitudesTickets
                                    .Include(u => u.SolIdUsuarioNavigation)
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
                            .Include(st => st.SolIdEstadoSolicitudNavigation)
                            .Include(st => st.SolIdTipoSolicitudNavigation)
                            .ToListAsync();

                oResponse.Data = list;
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
                    oUsuario.UsuFileNameComprobanteInscripcion = model.SolFileNameComprobanteInscripcion;

                    if (model.SolIdTipoSolicitud == 2)
                        oUsuario.UsuCorreoPersonalCuentaAnterior = model.SolCorreoPersonalCuentaNueva;
                    else if (model.SolIdTipoSolicitud == 3)
                        oUsuario.UsuNoCelularAnterior = model.SolNoCelularNuevo;

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
                    SolObservacionesSolicitud = model.SolObservacionesSolicitud,
                    SolIdEstadoSolicitud = model.SolIdEstadoSolicitud,
                    SolValidacionDatos = model.SolValidacionDatos,
                    SolEncuestaCalidadCalificacion = model.SolEncuestaCalidadCalificacion,
                    SolEncuestaCalidadComentarios = model.SolEncuestaCalidadComentarios,
                    SolFechaHoraEncuesta = model.SolFechaHoraEncuesta,
                    SolRespuestaDcyC = model.SolRespuestaDcyC,
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
                    oSolicitud.SolRespuestaDcyC = model.SolRespuestaDcyC;
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
                    oSolicitud.SolIdEstadoSolicitud = 5;
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
        }//ENCUESTA DE CALIDAD

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
                Random rnd = new Random();
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

            }catch(Exception ex)
            {
                oRespuesta.Message = ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return Ok(oRespuesta);
        }// GENERAR SOLICITUDES

        [HttpPatch("finalizar")]
        public async Task<IActionResult> FinalizarSolicitud(RequestDTO_FinalizarSolicitud oFinalizarSolicitud) // KeyValuePair<int, string> datos)
        {
            Response<object> oRespuesta = new();
            int guardados = 0;

            MtTbSolicitudesTicket? oSolicitud = null;
            
            try
            {
                oSolicitud = await _db.MtTbSolicitudesTickets!
                                    .Where(st => st.IdSolicitudTicket == oFinalizarSolicitud.IdSolicitud)
                                    .Include(u => u.SolIdUsuarioNavigation)
                                    .FirstAsync();

                //FindAsync(oFinalizarSolicitud.IdSolicitud); // (datos.Key)
                //db.Remove(oPersona);

                if (oSolicitud is not null)
                {
                    oSolicitud.SolIdEstadoSolicitud = oFinalizarSolicitud.Estado;
                    oSolicitud.SolRespuestaDcyC = oFinalizarSolicitud.Mensaje; // datos.Value;

                    _db.Entry(oSolicitud).State = EntityState.Modified;
                    guardados = await _db.SaveChangesAsync();
                }
                if (guardados == 1)
                {
                    oRespuesta.Success = 1;
                    await Notificar(oSolicitud!, (TipoEstadoSolicitud)oFinalizarSolicitud.Estado);
                }
            }
            catch (Exception ex)
            {
                oRespuesta.Message = ex.Message;
            }

            return Ok(oRespuesta);
        }
        // CANCELAR

        [HttpPost("notificar")]
        public async Task<IActionResult> Notificar(MtTbSolicitudesTicket solicitud, TipoEstadoSolicitud estado)
        {
            Response<object> oRespuesta = new();
            string correo_personal = solicitud.SolIdUsuarioNavigation.UsuCorreoPersonalCuentaNueva;
            
            if (Dominios.EsCorreoDePrueba(correo_personal))
            {
                oRespuesta.Success = 1;
                return Ok(oRespuesta);
            }

            try
            {
                ComponentRenderer renderer = new ComponentRenderer(_serviceProvider);

                Dictionary<string, object?> variables_correo = new Dictionary<string, object?>
                {
                    { "solicitud", solicitud }
                };

                RequestDTO_SendEmail correo = new()
                {
                    EmailTo = correo_personal,
                };

                RequestDTO_SendWhatsApp mensaje = new()
                {
                    Number = solicitud.SolIdUsuarioNavigation.UsuNoCelularNuevo.Replace(" ", string.Empty)
                };

                switch(estado)
                {
                    case TipoEstadoSolicitud.ATENDIDA:
                        correo.Subject = "Su solcicitud ha sido atendida por la mesa de control";

                        switch ((TipoPersonal)solicitud.SolIdUsuarioNavigation.UsuIdTipoPersonal)
                        {
                            case TipoPersonal.ALUMNO:
                            case TipoPersonal.EGRESADO:
                            case TipoPersonal.MAESTRIA:
                                correo.Body = await renderer.GetHTML<AtendidoAlumnoYEgresado>(variables_correo);
                                break;
                            default:
                                correo.Body = await renderer.GetHTML<AtendidoPersonal>(variables_correo);
                                break;
                        }

                        mensaje.Message = await renderer.GetHTML<AtendidoWA>(variables_correo);

                        break;

                    case TipoEstadoSolicitud.CANCELADA:
                        correo.Subject = "Su solcicitud ha sido cancelada";
                        correo.Body = await renderer.GetHTML<Cancelada>(variables_correo);

                        mensaje.Message = await renderer.GetHTML<CanceladoWA>(variables_correo);
                        break;
                }

                // HACER ENVÍOS SIN ESPERARSE A SU RESULTADO
                _ = Task.Run(() => _servicioCorreo.SendEmail(correo));
                _ = Task.Run(() => _servicioWA.SendWhatsAppAsync(mensaje));
            }
            catch (Exception ex)
            {
                oRespuesta.Message = ex.Message;
            }

            return Ok(oRespuesta);
        }
    }
}
