using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net.Http.Headers;

using CorreosInstitucionales.Shared.CapaEntities.Response;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers.RepositorioFiles
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepositoryFilesController(IHostEnvironment hostEnvironment, IWebHostEnvironment webHostEnvironment) : ControllerBase
    {
        private readonly IHostEnvironment _hostEnvironment = hostEnvironment;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        [HttpPost("[action]/{folder}/{id}/{fileName}/{guid}")]
        public async Task<IActionResult> UploadSingleFile(string folder, int id, string fileName, Guid guid, IFormFile file)
        {
            Response<object> oResponse = new();

            try
            {
                if (file.Length > 0 && file != null)
                {
                    // var carpeta = Path.Combine(_hostEnvironment.ContentRootPath, $@"wwwroot\repositorio\{id}_{guid}");
                    folder = Path.Combine(_webHostEnvironment.ContentRootPath, $"wwwroot/repositorio/{folder}/{id}"); // Development & Deployment wwwroot in Server
                    // var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    fileName = $"{id}_{Path.GetFileNameWithoutExtension(fileName)}_{guid}{Path.GetExtension(file.FileName).ToLower()}";

                    string dirPath = Path.Combine(folder, fileName);

                    if (!System.IO.File.Exists(dirPath))
                    {
                        using FileStream oFileStream = new(dirPath, FileMode.Create, FileAccess.Write); // System.IO.File.Create(dirPath);
                        await file.OpenReadStream().CopyToAsync(oFileStream);
                    }
                }

                oResponse.Success = 1;
            }
            catch (Exception ex)
            {
                Response.Clear();
                Response.StatusCode = 200;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File removed successfully";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = ex.Message;
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpPost("[action]/{folder}/{id}/{fileName}/{guid}")]
        public async Task<IActionResult> UploadMultipleFiles(string folder, int id, string fileName, Guid guid, IList<IFormFile> files)
        {
            Response<object> oResponse = new();

            try
            {
                if (files.Count < 1 || files.IsNullOrEmpty())
                    return BadRequest(oResponse);

                foreach (var file in files)
                {
                    // var carpeta = Path.Combine(_hostEnvironment.ContentRootPath, $@"wwwroot\repositorio\{id}_{guid}");
                    folder = Path.Combine(_webHostEnvironment.ContentRootPath, $"wwwroot/repositorio/{folder}/{id}"); // Development & Deployment wwwroot in Server
                    // var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    fileName = $"{id}_{fileName}_{guid}{Path.GetExtension(file.FileName).ToLower()}";
                    long size = file.Length;

                    string dirPath = Path.Combine(folder, fileName);

                    if (!System.IO.File.Exists(dirPath))
                    {
                        using FileStream oFileStream = new(dirPath, FileMode.Create, FileAccess.Write); // System.IO.File.Create(dirPath);
                        await file.OpenReadStream().CopyToAsync(oFileStream);
                    }
                }

                oResponse.Success = 1;
            }
            catch (Exception ex)
            {
                Response.Clear();
                Response.StatusCode = 204;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File failed to upload";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = ex.Message;
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpPost("[action]")]
        public async Task RemoveFile(IList<IFormFile> UploadFiles)
        {
            try
            {
                var filename = _hostEnvironment.ContentRootPath + $"/{UploadFiles[0].FileName}";

                if (System.IO.File.Exists(filename))
                    System.IO.File.Delete(filename);
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 200;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File removed successfully";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }
        }
    }
}
