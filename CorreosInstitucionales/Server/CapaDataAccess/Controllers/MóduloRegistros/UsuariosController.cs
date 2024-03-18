using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using CorreosInstitucionales.Shared.CapaTools;
using CorreosInstitucionales.Shared.Constantes;
using CorreosInstitucionales.Shared;
using CorreosInstitucionales.Shared.Utils;
using CorreosInstitucionales.Client.CapaPresentationComponentsPagesUI_UX.MóduloCatálogos;
using CorreosInstitucionales.Client.CapaPresentationComponentsPagesUI_UX.MóduloRegistros;

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
                    list = await _db.MpTbUsuarios.Where(u => u.UsuStatus.Equals(filterByStatus)).ToListAsync();
                                    //   .OrderByDescending(x => x.Id)
                                    //   .Skip((actualPage - 1) * Utilities.REGISTERSPERPAGE)
                                    //   .Take(Utilities.REGISTERSPERPAGE)
                                    //   .ToList();
                else
                    list = await _db.MpTbUsuarios.ToListAsync();

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
                    UsuNombre = model.UsuNombre.ToUpper(),
                    UsuPrimerApellido = model.UsuPrimerApellido.ToUpper(),
                    UsuSegundoApellido = model.UsuSegundoApellido == null ? null: model.UsuSegundoApellido.ToUpper(),
                    UsuCurp = model.UsuCurp.ToUpper(),
                    UsuFileNameCurp = model.UsuFileNameCurp,
                    UsuNoCelularAnterior = null,
                    UsuNoCelularNuevo = model.UsuNoCelularNuevo,
                    // DATOS ACADÉMICOS
                    UsuBoletaAlumno = model.UsuBoletaAlumno,
                    UsuBoletaMaestria = model.UsuBoletaMaestria,
                    UsuIdCarrera = model.UsuIdCarrera,
                    UsuSemestre = model.UsuSemestre,
                    UsuAñoEgreso = model.UsuAñoEgreso,
                    UsuFileNameComprobanteInscripcion = model.UsuFileNameComprobanteInscripcion,
                    // DATOS LABORALES
                    UsuNumeroEmpleado = model.UsuNumeroEmpleado,
                    UsuIdAreaDepto = model.UsuIdAreaDepto,
                    UsuNoExtensionAnterior = model.UsuNoExtensionAnterior,
                    UsuNoExtension = model.UsuNoExtension,
                    // DATOS DE LAS CREDENCIALES DE LA CUENTA EN LA APP
                    UsuCorreoPersonalCuentaAnterior = model.UsuCorreoPersonalCuentaAnterior,
                    UsuCorreoPersonalCuentaNueva = model.UsuCorreoPersonalCuentaNueva,
                    UsuContraseña = Encrypt.GetSHA256(model.UsuContraseña),
                    UsuRecuperarContraseña = false,
                    // DATOS DEL CORREO INSTITUCIONAL
                    UsuCorreoInstitucionalCuenta = model.UsuCorreoInstitucionalCuenta,
                    UsuCorreoInstitucionalContraseña = model.UsuCorreoInstitucionalContraseña,
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
                    oUsuario.UsuNombre = model.UsuNombre.ToUpper();
                    oUsuario.UsuPrimerApellido = model.UsuPrimerApellido.ToUpper();
                    oUsuario.UsuSegundoApellido = model.UsuSegundoApellido?.ToUpper();
                    oUsuario.UsuCurp = model.UsuCurp.ToUpper();
                    oUsuario.UsuFileNameCurp = model.UsuFileNameCurp;
                    oUsuario.UsuNoCelularAnterior = model.UsuNoCelularAnterior;
                    oUsuario.UsuNoCelularNuevo = model.UsuNoCelularNuevo;
                    // DATOS ACADÉMICOS
                    oUsuario.UsuBoletaAlumno = model.UsuBoletaAlumno;
                    oUsuario.UsuBoletaMaestria = model.UsuBoletaMaestria;
                    oUsuario.UsuIdCarrera = model.UsuIdCarrera;
                    oUsuario.UsuSemestre = model.UsuSemestre;
                    oUsuario.UsuAñoEgreso = model.UsuAñoEgreso;
                    oUsuario.UsuFileNameComprobanteInscripcion = model.UsuFileNameComprobanteInscripcion;
                    // DATOS LABORALES
                    oUsuario.UsuNumeroEmpleado = model.UsuNumeroEmpleado;
                    oUsuario.UsuIdAreaDepto = model.UsuIdAreaDepto;
                    oUsuario.UsuNoExtensionAnterior = model.UsuNoExtensionAnterior;
                    oUsuario.UsuNoExtension = model.UsuNoExtension;
                    // DATOS DE LAS CREDENCIALES DE LA CUENTA EN LA APP
                    oUsuario.UsuCorreoPersonalCuentaAnterior = model.UsuCorreoPersonalCuentaAnterior;
                    oUsuario.UsuCorreoPersonalCuentaNueva = model.UsuCorreoPersonalCuentaNueva;
                    oUsuario.UsuContraseña = model.UsuContraseña;
                    oUsuario.UsuRecuperarContraseña = false;
                    // DATOS DEL CORREO INSTITUCIONAL
                    oUsuario.UsuCorreoInstitucionalCuenta = model.UsuCorreoInstitucionalCuenta;
                    oUsuario.UsuCorreoInstitucionalContraseña = model.UsuCorreoInstitucionalContraseña;
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

                    oUsuario.UsuContraseña = Encrypt.GetSHA256(tmpPassword);
                    oUsuario.UsuRecuperarContraseña = true;

                    _db.Entry(oUsuario).State = EntityState.Modified;
                    await _db.SaveChangesAsync();

                    oRespuesta.Success = 1;
                    oRespuesta.Message = $"{tmpPassword}";
                    oRespuesta.Data = oUsuario;
                }
                else
                    oRespuesta.Message = "No se encuentró registrado el Correo Electrónico y/o CURP. Si el problema persisite acudir a la Unidad Informática (UDI).";
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
                    oUsuario.UsuContraseña = Encrypt.GetSHA256(newPassword);
                    oUsuario.UsuRecuperarContraseña = false;

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

        [HttpPut("generar_random/{cantidad}")]
        public async Task<IActionResult> GenerarUsuarios(int cantidad = 10)
        {
            Response<string> oResponse = new() { Data = string.Empty };

            Random rnd = new Random();
            string id;
            MpTbUsuario tmp;

            string basedir = ServerFS.GetBaseDir(true);

            RandomItemPicker<string> nombres_h = new RandomItemPicker<string>($"{basedir}/assets/nombres_h.txt");
            RandomItemPicker<string> nombres_m = new RandomItemPicker<string>($"{basedir}/assets/nombres_m.txt");
            RandomItemPicker<string> apellidos = new RandomItemPicker<string>($"{basedir}/assets/apellidos.txt");

            RandomItemPicker<TipoPersonal> rol = new RandomItemPicker<TipoPersonal>
            (
                new RandomItem<TipoPersonal>[]
                {
                    new RandomItem<TipoPersonal>(50,    TipoPersonal.ALUMNO),
                    new RandomItem<TipoPersonal>(10,    TipoPersonal.EGRESADO),
                    new RandomItem<TipoPersonal>(10,    TipoPersonal.MAESTRIA),
                    new RandomItem<TipoPersonal>(10,    TipoPersonal.DOCENTE),
                    new RandomItem<TipoPersonal>(10,    TipoPersonal.HONORARIOS),
                    new RandomItem<TipoPersonal>(5,     TipoPersonal.ADMINISTRATIVO),
                    new RandomItem<TipoPersonal>(5,     TipoPersonal.PLATAFORMA_SACI),
                }
            );

            bool es_hombre = rnd.NextDouble() >= 0.5f;//50%
            bool tiene_varios_nombres = rnd.NextDouble() >= 0.5f;//50%
            bool tiene_dos_apellidos = rnd.NextDouble() <= 0.85;//85% SI, 20% NO

            TipoPersonal tipo_personal = TipoPersonal.NINGUNO;

            bool es_alumno = false;

            DateOnly fecha_nacimiento;
            int anio_nacimiento = 1990;
            int anios_estudio = 0;
            int edad = 0;

            try
            {
                for (int i = 0; i < cantidad; i++)
                {
                    id = Guid.NewGuid().ToString();
                    tipo_personal = rol.GetRandomItem();

                    es_alumno = false;

                    edad = 21;// rnd.Next(25, 80);
                    anios_estudio = rnd.Next(0, 5);

                    tmp = new MpTbUsuario()
                    {
                        UsuFechaHoraAlta = DateTime.Now,
                        UsuIdRol = (int)tipo_personal,
                        UsuFileNameCurp = $"CURP_{id}.pdf",
                        UsuFileNameComprobanteInscripcion = "-",
                        UsuNoExtension = "0",
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

                    switch(tipo_personal)
                    {
                        case TipoPersonal.ALUMNO:
                            es_alumno = true;
                            
                            edad = rnd.Next(18, 25);
                            break;

                        case TipoPersonal.EGRESADO:
                            es_alumno = true;
                            edad = rnd.Next(21, 35);
                            anios_estudio = rnd.Next(4, 6);
                            break;

                        case TipoPersonal.MAESTRIA:
                            es_alumno = true;
                            edad = rnd.Next(30, 50);
                            anios_estudio = rnd.Next(4, 15);
                            break;
                    }

                    anio_nacimiento = DateTime.Now.Year - edad - anios_estudio;

                    fecha_nacimiento = new DateOnly(anio_nacimiento, rnd.Next(1, 12), 1);



                    if(es_alumno)
                    {
                        tmp.UsuFileNameComprobanteInscripcion = $"COMPROBANTE_INSCRIPCION_{id}.pdf";
                    }



                    await _db.MpTbUsuarios.AddAsync(tmp);
                    await _db.SaveChangesAsync();

                }//FOR

                oResponse.Success = 1;
            }
            catch ( Exception ex )
            {
                oResponse.Message = ex.Message;
                oResponse.Data = ex.StackTrace;
            }
            
            return Ok(oResponse);
        }//GenerarUsuarios
    }
}
