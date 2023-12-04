using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepositorioFilesController : ControllerBase
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RepositorioFilesController(IHostEnvironment hostEnvironment, IWebHostEnvironment webHostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("{repoFolder}/{subFolder}")]
        public async Task<IActionResult> SaveFile(string repoFolder, string subFolder, IList<IFormFile> UploadFiles)
        {
            Response<object> oResponse = new();
            long size = 0;

            try
            {
                foreach (var file in UploadFiles)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    //fileName = $@"C:\Users\Win10\Documents\GitHub\SITI_NET\siti\Client\wwwroot\Repositorio\{repoFolder}\{subFolder}\{fileName}";
                    fileName = $@"C:\Users\l_alo\Documents\GitHub\siti_NET\siti\Client\wwwroot\Repositorio\{repoFolder}\{subFolder}\{fileName}";

                    //fileName = _hostEnvironment.ContentRootPath + $@"\CapaDataAccess\Repositorio\{repoFolder}\{subFolder}\{fileName}";
                    // var file = System.IO.Path.Combine(_webHostEnvironment.ContentRootPath, oModel.IdFile, oModel.File.FileName);

                    size += (int)file.Length;

                    if (!System.IO.File.Exists(fileName))
                    {
                        using FileStream fs = System.IO.File.Create(fileName);
                        await file.CopyToAsync(fs);
                        await file.CopyToAsync(new FileStream(fileName, FileMode.Create));
                        fs.Flush();
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
                var filename = _hostEnvironment.ContentRootPath + $@"\{UploadFiles[0].FileName}";
                if (System.IO.File.Exists(filename))
                {
                    System.IO.File.Delete(filename);
                }
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
