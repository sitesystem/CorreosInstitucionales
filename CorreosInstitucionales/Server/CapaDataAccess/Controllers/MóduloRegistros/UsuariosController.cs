using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using CorreosInstitucionales.Shared.CapaTools;
using CorreosInstitucionales.Shared.Constantes;
using CorreosInstitucionales.Shared;
using CorreosInstitucionales.Shared.Utils;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers.MóduloRegistros
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UsuariosController(DbCorreosInstitucionalesUpiicsaContext db, IHostEnvironment hostEnvironment, IWebHostEnvironment webHostEnvironment) : ControllerBase
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;
        private readonly IHostEnvironment _hostEnvironment = hostEnvironment;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllDataByStatus(bool filterByStatus)
        {
            Response<List<MpTbUsuario>> oResponse = new();

            try
            {
                var list = new List<MpTbUsuario>();

                if (filterByStatus)
                    list = await _db.MpTbUsuarios.Where(u => u.UsuStatus.Equals(filterByStatus))
                                                 .Include(r => r.UsuIdRolNavigation)
                                                 .Include(tp => tp.UsuIdTipoPersonalNavigation)
                                                 .Include(ce => ce.UsuIdCarreraNavigation)
                                                 .Include(ad => ad.UsuIdAreaDeptoNavigation)
                                                 .ToListAsync();
                                    //   .OrderByDescending(x => x.Id)
                                    //   .Skip((actualPage - 1) * Utilities.REGISTERSPERPAGE)
                                    //   .Take(Utilities.REGISTERSPERPAGE)
                                    //   .ToList();
                else
                    list = await _db.MpTbUsuarios.Include(r => r.UsuIdRolNavigation)
                                                 .Include(tp => tp.UsuIdTipoPersonalNavigation)
                                                 .Include(ce => ce.UsuIdCarreraNavigation)
                                                 .Include(ad => ad.UsuIdAreaDeptoNavigation)
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

        [HttpGet("filterById/{id}")]
        public async Task<IActionResult> GetDataById(int id)
        {
            Response<MpTbUsuario> oResponse = new();

            try
            {
                var item = await _db.MpTbUsuarios.FindAsync(id);
                oResponse.Success = 1;
                oResponse.Data = item;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpGet("filterByEmailCURP/{correo}/{curp}")]
        public async Task<IActionResult> ValidateByEmailCURP(string correo, string curp)
        {
            Response<MpTbUsuario> oResponse = new();

            try
            {
                var list = await _db.MpTbUsuarios
                                    .Where(u => u.UsuCorreoPersonalCuentaNueva == correo || u.UsuCurp == curp)
                                    .Where(u => u.UsuStatus == true)
                                    .FirstOrDefaultAsync();
                if (list is null)
                    oResponse.Success = 1;
                else
                    oResponse.Data = list;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpPost]
        public async Task<IActionResult> AddData(RequestDTO_Usuario model)
        {
            Response<object> oResponse = new();
            CreatedAtActionResult oCreatedAtActionResult;

            try
            {
                MpTbUsuario oUsuario = new()
                {
                    // DATOS ID DEL USUARIO
                    IdUsuario = model.IdUsuario,
                    UsuIdRol = model.UsuIdRol,
                    UsuIdTipoPersonal = model.UsuIdTipoPersonal,
                    // UsuToken = Guid.NewGuid().ToString("D"),
                    // DATOS PERSONALES
                    UsuNombre = model.UsuNombre.ToUpper().Trim(),
                    UsuPrimerApellido = model.UsuPrimerApellido.ToUpper().Trim(),
                    UsuSegundoApellido = model.UsuSegundoApellido == null ? null: model.UsuSegundoApellido.ToUpper().Trim(),
                    UsuCurp = model.UsuCurp.ToUpper(),
                    UsuFileNameCurp = model.UsuFileNameCurp,
                    UsuNoCelularAnterior = null,
                    UsuNoCelularNuevo = model.UsuNoCelularNuevo,
                    // DATOS ACADÉMICOS
                    UsuBoletaAlumno = model.UsuBoletaAlumno,
                    UsuBoletaMaestria = model.UsuBoletaMaestria,
                    UsuIdCarrera = model.UsuIdCarrera,
                    UsuSemestre = model.UsuSemestre,
                    UsuAnioEgreso = model.UsuAnioEgreso,
                    UsuFileNameComprobanteInscripcion = model.UsuFileNameComprobanteInscripcion,
                    // DATOS LABORALES
                    UsuNumeroEmpleado = model.UsuNumeroEmpleado,
                    UsuIdAreaDepto = model.UsuIdAreaDepto,
                    UsuNoExtensionAnterior = model.UsuNoExtensionAnterior,
                    UsuNoExtension = model.UsuNoExtension,
                    // DATOS DE LAS CREDENCIALES DE LA CUENTA EN LA APP
                    UsuCorreoPersonalCuentaAnterior = model.UsuCorreoPersonalCuentaAnterior,
                    UsuCorreoPersonalCuentaNueva = model.UsuCorreoPersonalCuentaNueva.Trim(),
                    UsuContrasenia = Encrypt.GetSHA256(model.UsuContrasenia),
                    UsuRecuperarContrasenia = false,
                    // DATOS DEL CORREO INSTITUCIONAL
                    UsuCorreoInstitucionalCuenta = model.UsuCorreoInstitucionalCuenta?.Trim(),
                    UsuCorreoInstitucionalContrasenia = model.UsuCorreoInstitucionalContrasenia,
                    // OTROS DATOS
                    UsuFechaHoraAlta = DateTime.Now,
                    UsuStatus = model.UsuStatus,
                    // DATOS FK NAVIGATION
                    UsuIdAreaDeptoNavigation = null,
                    UsuIdCarreraNavigation = null,
                    UsuIdRolNavigation = null,
                    UsuIdTipoPersonalNavigation = null
                };

                await _db.MpTbUsuarios.AddAsync(oUsuario);
                await _db.SaveChangesAsync();

                oResponse.Success = 1;

                oCreatedAtActionResult = CreatedAtAction(nameof(AddData), new { id = oUsuario.IdUsuario }, oUsuario);
                oResponse.Message = oUsuario.IdUsuario.ToString(); // PK ID Único del Usuario Creado o dado de Alta
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpPut]
        public async Task<IActionResult> EditData(RequestDTO_Usuario model)
        {
            Response<object> oRespuesta = new();

            try
            {
                MpTbUsuario? oUsuario = await _db.MpTbUsuarios.FindAsync(model.IdUsuario);

                if (oUsuario != null)
                {
                    // DATOS ID DEL USUARIO
                    oUsuario.UsuIdRol = model.UsuIdRol;
                    oUsuario.UsuIdTipoPersonal = model.UsuIdTipoPersonal;
                    // DATOS PERSONALES
                    oUsuario.UsuNombre = model.UsuNombre.ToUpper().Trim();
                    oUsuario.UsuPrimerApellido = model.UsuPrimerApellido.ToUpper().Trim();
                    oUsuario.UsuSegundoApellido = model.UsuSegundoApellido?.ToUpper().Trim();
                    oUsuario.UsuCurp = model.UsuCurp.ToUpper().Trim();
                    oUsuario.UsuFileNameCurp = model.UsuFileNameCurp;
                    oUsuario.UsuNoCelularAnterior = model.UsuNoCelularAnterior;
                    oUsuario.UsuNoCelularNuevo = model.UsuNoCelularNuevo;
                    // DATOS ACADÉMICOS
                    oUsuario.UsuBoletaAlumno = model.UsuBoletaAlumno;
                    oUsuario.UsuBoletaMaestria = model.UsuBoletaMaestria;
                    oUsuario.UsuIdCarrera = model.UsuIdCarrera;
                    oUsuario.UsuSemestre = model.UsuSemestre;
                    oUsuario.UsuAnioEgreso = model.UsuAnioEgreso;
                    oUsuario.UsuFileNameComprobanteInscripcion = model.UsuFileNameComprobanteInscripcion;
                    // DATOS LABORALES
                    oUsuario.UsuNumeroEmpleado = model.UsuNumeroEmpleado;
                    oUsuario.UsuIdAreaDepto = model.UsuIdAreaDepto;
                    oUsuario.UsuNoExtensionAnterior = model.UsuNoExtensionAnterior;
                    oUsuario.UsuNoExtension = model.UsuNoExtension;
                    // DATOS DE LAS CREDENCIALES DE LA CUENTA EN LA APP
                    oUsuario.UsuCorreoPersonalCuentaAnterior = model.UsuCorreoPersonalCuentaAnterior;
                    oUsuario.UsuCorreoPersonalCuentaNueva = model.UsuCorreoPersonalCuentaNueva.Trim();
                    oUsuario.UsuContrasenia = model.UsuContrasenia;
                    oUsuario.UsuRecuperarContrasenia = false;
                    // DATOS DEL CORREO INSTITUCIONAL
                    oUsuario.UsuCorreoInstitucionalCuenta = model.UsuCorreoInstitucionalCuenta.Trim();
                    oUsuario.UsuCorreoInstitucionalContrasenia = model.UsuCorreoInstitucionalContrasenia;
                    // OTROS DATOS
                    // oUsuario.UsuFechaHoraAlta = DateTime.UtcNow;
                    oUsuario.UsuStatus = true;
                    // DATOS FK NAVIGATION
                    oUsuario.UsuIdAreaDeptoNavigation = null;
                    oUsuario.UsuIdCarreraNavigation = null;
                    oUsuario.UsuIdRolNavigation = null;
                    oUsuario.UsuIdTipoPersonalNavigation = null;

                    _db.Entry(oUsuario).State = EntityState.Modified;
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

        [HttpPut("resetPassword/{correoPersonal}/{curp}")]
        public async Task<IActionResult> ResetPassword(string correoPersonal, string curp)
        {
            Response<MpTbUsuario> oRespuesta = new();
            try
            {
                MpTbUsuario? oUsuario = await _db.MpTbUsuarios.Where(u => u.UsuCorreoPersonalCuentaNueva == correoPersonal && u.UsuCurp == curp && u.UsuStatus == true)
                                                              .FirstOrDefaultAsync();

                if (oUsuario != null)
                {
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@#$%&";
                    string tmpPassword = new(Enumerable.Repeat(chars, 10).Select(s => s[new Random().Next(s.Length)]).ToArray());

                    oUsuario.UsuContrasenia = Encrypt.GetSHA256(tmpPassword);
                    oUsuario.UsuRecuperarContrasenia = true;

                    _db.Entry(oUsuario).State = EntityState.Modified;
                    await _db.SaveChangesAsync();

                    oRespuesta.Success = 1;
                    oRespuesta.Message = $"{tmpPassword}";
                    oRespuesta.Data = oUsuario;
                }
                else
                    oRespuesta.Message = "Verifique sus datos, no se encontró registrado el Correo Electrónico Personal y/o CURP. Si el problema persisite acudir a la Unidad Informática (UDI).";
            }
            catch (Exception ex)
            {
                oRespuesta.Message = ex.Message;
            }

            return Ok(oRespuesta);
        }

        [HttpPut("changePassword/{id}/{newPassword}")]
        public async Task<IActionResult> ChangePassword(int id, string newPassword)
        {
            Response<object> oRespuesta = new();

            try
            {
                MpTbUsuario? oUsuario = await _db.MpTbUsuarios.FindAsync(id);

                if (oUsuario != null)
                {
                    oUsuario.UsuContrasenia = Encrypt.GetSHA256(newPassword);
                    oUsuario.UsuRecuperarContrasenia = false;

                    _db.Entry(oUsuario).State = EntityState.Modified;
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

        //[HttpDelete("{Id}")]
        [HttpPut("editByIdStatus/{id}/{isActivate}")]
        public async Task<IActionResult> EnableDisableDataById(int id, bool isActivate)
        {
            Response<object> oRespuesta = new();

            try
            {
                MpTbUsuario? oUsuario = await _db.MpTbUsuarios.FindAsync(id);
                //db.Remove(oPersona);

                if (oUsuario != null)
                {
                    oUsuario.UsuStatus = isActivate;
                    _db.Entry(oUsuario).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                }

                oRespuesta.Success = 1;
            }
            catch (Exception ex)
            {
                oRespuesta.Message = ex.Message;
            }

            return Ok(oRespuesta);
        }//editByIdStatus

        [HttpGet("generar_random/{cantidad}")]
        public async Task<IActionResult> GenerarUsuarios(int cantidad = 10)
        {
            Response<string> oResponse = new() { Data = string.Empty };

            Random rnd = new Random();
            string id;
            MpTbUsuario tmp;

            string basedir = ServerFS.GetBaseDir(true);

            RandomItemPicker<string> nombres_h = new ($"{basedir}/assets/nombres_h.txt");
            RandomItemPicker<string> nombres_m = new ($"{basedir}/assets/nombres_m.txt");
            RandomItemPicker<string> apellidos = new ($"{basedir}/assets/apellidos.txt");

            RandomItemPicker<TipoPersonal> rol = new RandomItemPicker<TipoPersonal>
            (
                new RandomItem<TipoPersonal>[]
                {
                    new (50,    TipoPersonal.ALUMNO),
                    new (10,    TipoPersonal.EGRESADO),
                    new (10,    TipoPersonal.MAESTRIA),
                    new (10,    TipoPersonal.DOCENTE),
                    new (10,    TipoPersonal.HONORARIOS),
                    new (5,     TipoPersonal.ADMINISTRATIVO),
                    new (5,     TipoPersonal.PLATAFORMA_SACI),
                }
            );

            RandomItemPicker<string> estados = new
            (
                [
                    "AS", "BC", "BS", "CC", "CL", "CM", "CS", "CH",
                    "DG", "GT", "GR", "HG", "JC", "MC", "MN", "MS",
                    "NT", "NL", "OC", "PL", "QT", "QR", "SP", "SL",
                    "SR", "TC", "TS", "TL", "VZ", "YN", "ZS", "DF",
                    "NE"
                ]
            );

            bool es_hombre = rnd.NextDouble() >= 0.5f;//50%
            bool tiene_varios_nombres = rnd.NextDouble() >= 0.5f;//50%
            bool tiene_dos_apellidos = rnd.NextDouble() <= 0.85;//85% SI, 20% NO

            TipoPersonal tipo_personal = TipoPersonal.NINGUNO;

            bool es_alumno = false;

            DateOnly fecha_nacimiento;
            int anio_nacimiento = 1990;
            int anios_estudio = 0;
            int anios_egreso = 0;

            List<MpTbUsuario> registros = [];

            try
            {
                for (int i = 0; i < cantidad; i++)
                {
                    id = Guid.NewGuid().ToString();
                    tipo_personal = rol.GetRandomItem();

                    es_alumno = false;

                    anios_estudio = rnd.Next(0, 5);

                    tmp = new MpTbUsuario()
                    {
                        UsuSegundoApellido = null,
                        UsuFechaHoraAlta = DateTime.Now,
                        UsuIdRol = 2,
                        UsuIdTipoPersonal= (int)tipo_personal,
                        UsuFileNameCurp = $"CURP_{id}.pdf",
                        UsuFileNameComprobanteInscripcion = "-",
                        UsuNoExtension = "0",
                        UsuBoletaMaestria = "B000000",
                        UsuNumeroEmpleado = "0",
                        UsuIdAreaDepto = 1,
                        UsuIdCarrera = 8,
                        UsuStatus = true
                    };

                    tmp.UsuNombre = es_hombre ? nombres_h.GetRandomItem() : nombres_m.GetRandomItem();

                    if (tiene_varios_nombres)
                    {
                        tmp.UsuNombre += " " + (es_hombre ? nombres_h.GetRandomItem() : nombres_m.GetRandomItem());
                    }

                    tmp.UsuPrimerApellido = apellidos.GetRandomItem();

                    if (tiene_dos_apellidos)
                    {
                        tmp.UsuSegundoApellido = apellidos.GetRandomItem();
                    }
                                        
                    switch (tipo_personal)
                    {
                        case TipoPersonal.ALUMNO:
                            es_alumno = true;
                            anios_estudio = rnd.Next(0, 5);
                            anios_egreso = 0;

                            tmp.UsuSemestre = Math.Clamp(anios_estudio * 2, 1, 8).ToString();
                            break;

                        case TipoPersonal.EGRESADO:
                            es_alumno = true;
                            anios_estudio = rnd.Next(4, 6);
                            anios_egreso = rnd.Next(0, 20);

                            tmp.UsuSemestre = "8";
                            tmp.UsuAnioEgreso = DateTime.Now.Year - (anios_egreso + anios_estudio);
                            break;

                        case TipoPersonal.MAESTRIA:
                            es_alumno = true;
                            anios_estudio = rnd.Next(4, 6);
                            anios_egreso = rnd.Next(0, 20);

                            tmp.UsuAnioEgreso = DateTime.Now.Year - (anios_egreso + anios_estudio);
                            break;
                    }

                    anio_nacimiento = DateTime.Now.Year - (18 + anios_egreso + anios_estudio);

                    tmp.UsuBoletaAlumno = $"{DateTime.Now.Year - anios_estudio - anios_egreso}600{tmp.UsuNombre[0] % 10}{tmp.UsuPrimerApellido[0] % 10}{(tmp.UsuSegundoApellido ?? "X")[0] % 10}";

                    
                    fecha_nacimiento = new DateOnly(anio_nacimiento, rnd.Next(1, 12), 1);

                    tmp.UsuCurp = CURP.Generar(tmp.UsuNombre, tmp.UsuPrimerApellido, tmp.UsuSegundoApellido, fecha_nacimiento, es_hombre?'H':'M', estados.GetRandomItem());

                    tmp.UsuFileNameCurp = $"CURP_{id}.pdf";

                    if (es_alumno)
                    {
                        tmp.UsuIdCarrera = rnd.Next(1, 7);
                        tmp.UsuFileNameComprobanteInscripcion = $"COMPROBANTE_INSCRIPCION_{id}.pdf";
                    }

                    tmp.UsuContrasenia = Encrypt.GetSHA256(tmp.UsuCurp);
                    tmp.UsuCorreoPersonalCuentaNueva = "noreply@localhost";
                    tmp.UsuNoCelularNuevo = "55 00 00 00 00";

                    //await _db.MpTbUsuarios.AddAsync(tmp);
                    registros.Add(tmp);

                }//FOR

                await _db.MpTbUsuarios.AddRangeAsync(registros);
                await _db.SaveChangesAsync();

                oResponse.Success = 1;
            }
            catch ( Exception ex )
            {
                oResponse.Message = ex.Message;
                oResponse.Data = ex.StackTrace;

                if(ex.InnerException is not null)
                {
                    oResponse.Message += Environment.NewLine + ex.InnerException.Message;
                }
            }
            
            return Ok(oResponse);
        }//GenerarUsuarios
    }
}
