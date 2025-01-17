﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using CorreosInstitucionales.Shared.CapaEntities.Response;
using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaDataAccess;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers.MóduloCatálogos
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AnunciosController(DbCorreosInstitucionalesUpiicsaContext db) : ControllerBase
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;

        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllDataByStatus(bool filterByStatus)
        {
            Response<List<McCatAnuncio>> oResponse = new();

            try
            {
                var list = new List<McCatAnuncio>();

                if (filterByStatus)
                    list = AppCache.Anuncios
                        .Where(
                            a => 
                                a.AnuStatus &&
                                (
                                    a.AnuVisibleDesde == null && a.AnuVisibleHasta == null ||
                                    a.AnuVisibleDesde != null && a.AnuVisibleDesde >= DateTime.Now ||
                                    a.AnuVisibleHasta != null && a.AnuVisibleHasta <= DateTime.Now
                                )
                        )
                        .ToList();
                else
                    list = await _db.McCatAnuncios.ToListAsync();

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
            Response<McCatAnuncio> oResponse = new();

            try
            {
                var item = await _db.McCatAnuncios.FindAsync(id);
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
        public async Task<IActionResult> AddData(RequestViewModel_Anuncio model)
        {
            Response<object> oResponse = new();

            try
            {
                await _db.McCatAnuncios.AddAsync(model);
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
        public async Task<IActionResult> EditData(RequestViewModel_Anuncio model)
        {
            Response<object> oRespuesta = new();

            try
            {
                _db.Entry(model).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                oRespuesta.Success = 1;
            }
            catch (Exception ex)
            {
                oRespuesta.Message = ex.Message;
            }

            return Ok(oRespuesta);
        }

        [HttpPut("editByIdStatus/{id}/{isActivate}")]
        public async Task<IActionResult> EnableDisableDataById(int id, bool isActivate)
        {
            Response<object> oRespuesta = new();

            try
            {
                McCatAnuncio? oAnuncio = await _db.McCatAnuncios.FindAsync(id);
                //db.Remove(oAnuncio);

                if (oAnuncio != null)
                {
                    oAnuncio.AnuStatus = isActivate;
                    _db.Entry(oAnuncio).State = EntityState.Modified;
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
