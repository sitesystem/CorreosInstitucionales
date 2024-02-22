using CorreosInstitucionales.Shared;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers.ModuloEstadisticas
{
    [Route("api/[controller]")]
    [ApiController]

    public class EstadisticasController(DbCorreosInstitucionalesUpiicsaContext db) : Controller
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;

        [HttpGet("progreso/{yy_s}_{mm_s}_{dd_s}-{yy_e}_{mm_e}_{dd_e}")]
        public async Task<IActionResult> GetStatsByDates(
            int yy_s, int mm_s, int dd_s,
            int yy_e, int mm_e, int dd_e
            )
        {
            DateTime inicio = new DateTime(yy_s, mm_s, dd_s);
            DateTime fin = new DateTime(yy_e, mm_e, dd_e);
            
            /*
                progress        SolIdEstadoSolicitud
                1 pendiente     1,2
                2 progreso      3
                3 terminado     4,5
                4 cancelado     6
             */
            Response<List<IntDataItem>> oResponse = new();

            try
            {
                int pendientes = await _db.MtTbSolicitudesTickets.CountAsync
                (
                    st =>
                        st.SolFechaHoraCreacion.Date >= inicio.Date &&
                        st.SolFechaHoraCreacion.Date <= fin.Date &&
                        st.SolIdEstadoSolicitud == 1
                );

                int progreso = await _db.MtTbSolicitudesTickets.CountAsync
                (
                    st =>
                        st.SolFechaHoraCreacion.Date >= inicio.Date &&
                        st.SolFechaHoraCreacion.Date <= fin.Date &&
                        st.SolIdEstadoSolicitud == 3
                );

                int terminados = await _db.MtTbSolicitudesTickets.CountAsync
                (
                    st =>
                        st.SolFechaHoraCreacion.Date >= inicio.Date &&
                        st.SolFechaHoraCreacion.Date <= fin.Date &&
                        st.SolIdEstadoSolicitud == 4 ||
                        st.SolIdEstadoSolicitud == 5
                );

                int cancelados = await _db.MtTbSolicitudesTickets.CountAsync
                (
                    st =>
                        st.SolFechaHoraCreacion.Date >= inicio.Date &&
                        st.SolFechaHoraCreacion.Date <= fin.Date &&
                        st.SolIdEstadoSolicitud == 6
                );

                oResponse.Success = 1;
                oResponse.Data = new List<IntDataItem>()
                {
                    new IntDataItem(pendientes,    "pendientes"    ),
                    new IntDataItem(progreso,      "progreso"      ),
                    new IntDataItem(terminados,    "terminados"    ),
                    new IntDataItem(cancelados,    "cancelados"    )
                };
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

    }
}
