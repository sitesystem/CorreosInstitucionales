﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

using CorreosInstitucionales.Shared;
using CorreosInstitucionales.Shared.CapaEntities.Response;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers.ModuloEstadisticas
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadisticasController(DbCorreosInstitucionalesUpiicsaContext db) : Controller
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;

        [HttpGet("volumen/{yy_s}_{mm_s}_{dd_s}-{yy_e}_{mm_e}_{dd_e}")]
        public async Task<IActionResult> GetVolumeStatsByDates(
            int yy_s, int mm_s, int dd_s,
            int yy_e, int mm_e, int dd_e
            )
        {
            DateTime inicio = new(yy_s, mm_s, dd_s);
            DateTime fin = new(yy_e, mm_e, dd_e);

            Response<Dictionary<string, List<IntDataItem>>> oResponse = new()
            {
                Data = []
            };

            try
            {
                List<IntDataItem>? list = await _db.MtTbSolicitudesTickets
                    .Where(st =>
                        st.SolFechaHoraCreacion.Date >= inicio.Date &&
                        st.SolFechaHoraCreacion.Date <= fin.Date &&
                        (
                            st.SolIdEstadoSolicitud == 1 ||
                            st.SolIdEstadoSolicitud == 2
                        )
                    )
                    .OrderBy(st=>st.SolFechaHoraCreacion)
                    .GroupBy(st => st.SolFechaHoraCreacion.Date)
                    .Select(group => new IntDataItem(group.Count(), group.Key.ToString()))
                    .ToListAsync();

                oResponse.Data.Add("Pendientes", list ?? new());

                list = await _db.MtTbSolicitudesTickets
                        .Where(st =>
                            st.SolFechaHoraCreacion.Date >= inicio.Date &&
                            st.SolFechaHoraCreacion.Date <= fin.Date &&
                            (
                                st.SolIdEstadoSolicitud == 3
                            )
                        )
                        .OrderBy(st => st.SolFechaHoraCreacion)
                        .GroupBy(st => st.SolFechaHoraCreacion.Date)
                        .Select(group => new IntDataItem(group.Count(), group.Key.ToString()))
                        .ToListAsync();
                    
                oResponse.Data.Add("En proceso", list ?? new());

                list = await _db.MtTbSolicitudesTickets
                        .Where(st =>
                            st.SolFechaHoraCreacion.Date >= inicio.Date &&
                            st.SolFechaHoraCreacion.Date <= fin.Date &&
                            (
                                st.SolIdEstadoSolicitud == 4 ||
                                st.SolIdEstadoSolicitud == 5
                            )
                        )
                        .OrderBy(st => st.SolFechaHoraCreacion)
                        .GroupBy(st => st.SolFechaHoraCreacion.Date)
                        .Select(group => new IntDataItem(group.Count(), group.Key.ToString()))
                        .ToListAsync();

                oResponse.Data.Add("Atendidos", list ?? new());

                list = await _db.MtTbSolicitudesTickets
                        .Where(st =>
                            st.SolFechaHoraCreacion.Date >= inicio.Date &&
                            st.SolFechaHoraCreacion.Date <= fin.Date &&
                            (
                                st.SolIdEstadoSolicitud == 6
                            )
                        )
                        .OrderBy(st => st.SolFechaHoraCreacion)
                        .GroupBy(st => st.SolFechaHoraCreacion.Date)
                        .Select(group => new IntDataItem(group.Count(), group.Key.ToString()))
                        .ToListAsync();

                oResponse.Data.Add("Cancelados", list ?? new());

                oResponse.Success = 1;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }//VOLUMEN(?)

        [HttpGet("calidad/{yy_s}_{mm_s}_{dd_s}-{yy_e}_{mm_e}_{dd_e}")]
        public async Task<IActionResult> GetSatisfactionStatsByDates(
            int yy_s, int mm_s, int dd_s,
            int yy_e, int mm_e, int dd_e
            )
        {
            DateTime inicio = new DateTime(yy_s, mm_s, dd_s);
            DateTime fin = new DateTime(yy_e, mm_e, dd_e);

            Response<List<IntDataItem>> oResponse = new();

            Dictionary<string, string> calificaciones = new()
            {
                { "-1", "(Sin calificar)"},
                { "0", "Muy Malo"},
                { "1", "Bastante Malo"},
                { "2", "Malo"},
                { "3", "Regular"},
                { "4", "Bueno"},
                { "5", "Muy Bueno"},
            };

            try
            {
                List<IntDataItem>? list = await Task.Run(
                    () =>
                    {
                        return _db.MtTbSolicitudesTickets
                        .Where(st =>
                            st.SolFechaHoraCreacion.Date >= inicio.Date &&
                            st.SolFechaHoraCreacion.Date <= fin.Date
                        )
                        .GroupBy(st => st.SolEncuestaCalidadCalificacion)
                        .Select(group => new IntDataItem(group.Count(), group.Key.HasValue ? group.Key.Value.ToString() : "-1"))
                        .ToList();
                    }
                );

                //MAPEAR CALIFICACIONES
                for(int i = 0; i< list.Count; i++)
                {
                    list[i].Text = calificaciones[list[i].Text];
                }

                oResponse.Data = list;
                oResponse.Success = 1;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }//CALIDAD

        [HttpGet("diarias/{yy_s}_{mm_s}_{dd_s}-{yy_e}_{mm_e}_{dd_e}")]
        public async Task<IActionResult> GetDailyStatsByDates(
            int yy_s, int mm_s, int dd_s,
            int yy_e, int mm_e, int dd_e
            )
        {
            DateTime inicio = new(yy_s, mm_s, dd_s);
            DateTime fin = new(yy_e, mm_e, dd_e);

            Response<List<IntDataItem>> oResponse = new();

            try
            {
                List<IntDataItem>? list = await Task.Run(
                    () =>
                    {
                        return _db.MtTbSolicitudesTickets
                        .Where(st =>
                            st.SolFechaHoraCreacion.Date >= inicio.Date &&
                            st.SolFechaHoraCreacion.Date <= fin.Date
                        )
                        .GroupBy(st => st.SolFechaHoraCreacion.Date)
                        .Select(group => new IntDataItem(group.Count(), group.Key.ToString()))
                        .ToList();
                    }
                );

                oResponse.Data = list;
                oResponse.Success = 1;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }//DIARIAS

        [HttpGet("progreso/{yy_s}_{mm_s}_{dd_s}-{yy_e}_{mm_e}_{dd_e}")]
        public async Task<IActionResult> GetStatsByDates(
            int yy_s, int mm_s, int dd_s,
            int yy_e, int mm_e, int dd_e
            )
        {
            DateTime inicio = new(yy_s, mm_s, dd_s);
            DateTime fin = new(yy_e, mm_e, dd_e);
            
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
                        (
                            st.SolIdEstadoSolicitud == 1 ||
                            st.SolIdEstadoSolicitud == 2
                        )
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
                oResponse.Data = new List<IntDataItem>();

                if (pendientes > 0)
                {
                    oResponse.Data.Add(new IntDataItem(pendientes, "Pendientes"));
                }

                if (progreso > 0)
                {
                    oResponse.Data.Add(new IntDataItem(progreso, "En proceso"));
                }

                if (terminados > 0)
                {
                    oResponse.Data.Add(new IntDataItem(terminados, "Atendidos"));
                }

                if (cancelados > 0)
                {
                    oResponse.Data.Add(new IntDataItem(pendientes, "Cancelados"));
                }
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }//PROGRESO
    }
}
