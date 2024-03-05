using ClosedXML.Excel;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using CorreosInstitucionales.Shared.Utils;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Infrastructure;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using System.Text;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ArchivosController(DbCorreosInstitucionalesUpiicsaContext db) : Controller
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;

        private string? WriteZip(string filename, List<WebUtils.Link> files, string root_directory = "")
        {
            string z_name = string.Empty;
            string z_filename = string.Empty;
            string? log = null;

            StringBuilder sb = new();
            bool guardar_log = false;

            using (FileStream fs = new FileStream(filename, FileMode.CreateNew))
            {
                using (ZipArchive za = new ZipArchive(fs, ZipArchiveMode.Create, true))
                {
                    foreach (WebUtils.Link file in files)
                    {
                        z_filename = $"{root_directory}{file.Url}";

                        if (!System.IO.File.Exists(z_filename))
                        {
                            sb.AppendLine($"NO EXISTE EL ARCHIVO {z_filename}");
                            guardar_log = true;
                            continue;
                        }

                        z_name = file.Name == "#" ? Path.GetFileName(file.Url) : file.Name;

                        za.CreateEntryFromFile(z_filename,z_name);
                    }

                    if(guardar_log)
                    {
                        log = sb.ToString();

                        ZipArchiveEntry logfile = za.CreateEntry("log.txt");
                        using (Stream stream = logfile.Open())
                        {
                            using(StreamWriter sw = new StreamWriter(stream, Encoding.UTF8))
                            {
                                sw.Write(log);
                                sw.Flush();
                            }
                        }
                    }
                }

                fs.Position = 0;
            }

            return log;
        }

        private string GenerarNombreAdjunto(MtTbSolicitudesTicket solicitud, string archivo, string tipo = "ARCHIVO")
        {
            string ext = Path.GetExtension(archivo);
            string curp = solicitud.SolIdUsuarioNavigation.UsuCurp;
            string id = string.Format("{0:00000}", solicitud.IdSolicitudTicket);
            return $"{curp}_SOL{id}_{tipo}{ext}";
        }

        [HttpPost("xlsx/pendientes")]
        public async Task<IActionResult> GenerarPendientes_XLSX(int[] selected)
        {
            return await LlenarFormulario(selected, false);
        }

        [HttpPost("zip/pendientes")]
        public async Task<IActionResult> GenerarPendientes_ZIP(int[] selected)
        {
            return await LlenarFormulario(selected, true);
        }

        public async Task<IActionResult> LlenarFormulario(int[] selected, bool return_zip)
        {
            Guid id = Guid.NewGuid();
            string id_fecha = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            Response<List<WebUtils.Link>> oResponse = new() { Data = new() } ;
            List<MtTbSolicitudesTicket> pendientes = new List<MtTbSolicitudesTicket>();

            string base_directory = $"../Client/wwwroot";
            string filename = $"repositorio/pendientes/{id_fecha}_solicitud_alta_desbloqueo_{id}";
            string template_fn = $"assets/sol_alta_desbloqueo.xlsx";
            
            XLWorkbook wb = new XLWorkbook($"{base_directory}/{template_fn}");
            IXLWorksheet ws = wb.Worksheet(1);

            StringBuilder errors = new StringBuilder();
            bool save_log = false;

            int i = 5;
            string rol = string.Empty;
            string repositorio = string.Empty;

            List<WebUtils.Link> files = new();

            //FECHA
            ws.Cell("H2").Value = DateTime.Now.ToString("dd/MM/yyyy");

            /* ROLES - PSOIBLES VALORES:
             * ALUMNO
             * EGRESADO
             * DOCENTE
             * ADMINISTRATIVO
             * FUNCIONARIO
             * HONORARIOS
            */

            Dictionary<int, string> roles = new()
            {
                {1, "ALUMNO" },//ALUMNO
                {2, "EGRESADO" },//EGRESADO
                {3, "EGRESADO" },//ALUMNO MAESTRIA
                {4, "ADMINISTRATIVO" },//PAAE (ADMINISTRATIVO)
                {5, "DOCENTE" },//DOCENTE
                {6, "HONORARIOS" },//HONORARIOS
                {7, "FUNCIONARIO" },//PERSONAL UDI - <!> PREGUNTAR
            };

            string? id_externo_usuario = string.Empty;
            
            try
            {
                pendientes = await Task.Run(
                    ()=>_db.MtTbSolicitudesTickets.
                        Where(st => selected.Contains(st.IdSolicitudTicket)).
                        Include(st=>st.SolIdUsuarioNavigation).
                        Include(st=>st.SolIdTipoSolicitudNavigation).
                        ToList()
                );

                foreach(MtTbSolicitudesTicket solicitud in pendientes)
                {
                    repositorio = $"Repositorio/Solicitudes-Tickets/{solicitud.IdSolicitudTicket}/{solicitud.IdSolicitudTicket}_";

                    switch (solicitud.SolIdUsuarioNavigation.UsuIdTipoPersonal)
                    {
                        case 1: case 2: id_externo_usuario = solicitud.SolIdUsuarioNavigation.UsuBoletaAlumno; break;
                        case 3: id_externo_usuario = solicitud.SolIdUsuarioNavigation.UsuBoletaMaestria; break;
                        default: id_externo_usuario = solicitud.SolIdUsuarioNavigation.UsuNumeroEmpleado; break;
                    }

                    ws.Cell(i, 1).Value = solicitud.SolIdUsuarioNavigation.UsuNombre;
                    ws.Cell(i, 2).Value = solicitud.SolIdUsuarioNavigation.UsuPrimerApellido;
                    ws.Cell(i, 3).Value = solicitud.SolIdUsuarioNavigation.UsuSegundoApellido;
                    ws.Cell(i, 5).Value = solicitud.SolIdUsuarioNavigation.UsuCurp;
                    ws.Cell(i, 8).Value = solicitud.SolIdUsuarioNavigation.UsuCorreoPersonalCuentaNueva;
                    ws.Cell(i, 9).Value = solicitud.SolIdUsuarioNavigation.UsuCorreoInstitucionalCuenta;

                    ws.Cell(i, 4).Value = roles[solicitud.SolIdUsuarioNavigation.UsuIdTipoPersonal];
                    ws.Cell(i, 6).Value = id_externo_usuario;

                    ws.Cell(i, 7).Value = solicitud.SolIdUsuarioNavigation.UsuNoExtension;

                    IXLRange row = ws.Range(ws.Cell(i, 1), ws.Cell(i, 9));

                    row.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    row.Style.Border.SetOutsideBorderColor(XLColor.Black);

                    row.Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                    row.Style.Border.SetInsideBorderColor(XLColor.Black);


                    if(!string.IsNullOrEmpty(solicitud.SolCapturaCuentaBloqueada))
                    {
                        files.Add(
                            new WebUtils.Link(
                                    $"{repositorio}{solicitud.SolCapturaCuentaBloqueada}",
                                    GenerarNombreAdjunto
                                    (
                                        solicitud, 
                                        solicitud.SolCapturaCuentaBloqueada, 
                                        "CAPTURA_CUENTA_BLOQUEADA"
                                    )
                                )
                            );
                    }

                    if (!string.IsNullOrEmpty(solicitud.SolCapturaEscaneoAntivirus))
                    {
                        files.Add(
                            new WebUtils.Link(
                                    $"{repositorio}{solicitud.SolCapturaEscaneoAntivirus}",
                                    GenerarNombreAdjunto
                                    (
                                        solicitud,
                                        solicitud.SolCapturaEscaneoAntivirus,
                                        "CAPTURA_ESCANEO_ANTIVIRUS"
                                    )
                                )
                            );
                    }

                    if (!string.IsNullOrEmpty(solicitud.SolCapturaError))
                    {
                        files.Add(
                            new WebUtils.Link(
                                    $"{repositorio}{solicitud.SolCapturaError}",
                                    GenerarNombreAdjunto
                                    (
                                        solicitud,
                                        solicitud.SolCapturaError,
                                        "CAPTURA_MENSAJE_ERROR"
                                    )
                                )
                            );
                    }

                    i++;
                }

                ws.Columns(1, 10).AdjustToContents();

                wb.SaveAs($"{base_directory}/{filename}.xlsx");

                files.Add(new WebUtils.Link(filename + ".xlsx"));

                string? zip_errors = WriteZip($"{base_directory}/{filename}.zip", files, base_directory + "/" );

                if(zip_errors is not null)
                {
                    errors.AppendLine(zip_errors);
                    save_log = true;
                }
                
                if (return_zip)
                {
                    oResponse.Data = new List<WebUtils.Link>() {  new WebUtils.Link(filename + ".zip") };
                }
                else
                {
                    oResponse.Data = files;
                }
                
                oResponse.Success = 1;
            }
            catch (Exception ex)
            {
                errors.AppendLine(ex.Message);
                save_log = true;

                //Response.Clear();
                Response.StatusCode = 500;
            }

            if(save_log)
            {
                System.IO.File.WriteAllText($"{base_directory}/{filename}.txt", errors.ToString());

                oResponse.Message = errors.ToString();
                oResponse.Data.Add(new WebUtils.Link($"{base_directory}/{filename}.txt"));
            }

            return Ok(oResponse);
        }
    }
}
