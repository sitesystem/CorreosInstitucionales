using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using CorreosInstitucionales.Shared;
using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using CorreosInstitucionales.Shared.CapaTools;
using CorreosInstitucionales.Shared.Constantes;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolNotificaciones;
using System.Dynamic;
using Azure.Identity;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers.MóduloRegistros
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class UsuariosController(
        DbCorreosInstitucionalesUpiicsaContext db, 
        RSendNotificacionesService servicioNotificacion
        ) : ControllerBase
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;
        private readonly RSendNotificacionesService _servicioNotificacion = servicioNotificacion;

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

        [HttpGet("countData")]
        public async Task<IActionResult> GetCountData()
        {
            Response<object> oResponse = new();

            try
            {
                var count = await _db.MpTbUsuarios.CountAsync();

                oResponse.Message = count.ToString();
                oResponse.Success = 1;
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
                                    .Where(u => u.UsuCorreoPersonalCuentaActual == correo || u.UsuCurp == curp)
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
            Response<int> oResponse = new();
            //CreatedAtActionResult oCreatedAtActionResult;
            McCatPlantillas[] plantillas;
            PlantillaManager plantilla;

            try
            {
                model.UsuContrasenia = Encrypt.GetSHA256(model.UsuContrasenia);
                model.UsuFechaHoraAlta = DateTime.Now;
                model.UsuFechaHoraActualizacion = DateTime.Now;

                await _db.MpTbUsuarios.AddAsync(model);
                await _db.SaveChangesAsync();

                oResponse.Success = 1;
                oResponse.Data = model.IdUsuario; // PK ID Único del Usuario Creado o dado de Alta

                Response<Notificacion?> notificacion = PlantillaManager.GetNotificacion
                (
                    new()
                    {
                        { "usuario", model }
                    },
                    1, PlantillaManager.FILTRO_ALTA_USUARIO
                );

                Response<string> response = await _servicioNotificacion.EnviarAsync(notificacion.Data!);

                if (response.Success != 1)
                {
                    oResponse.Message += Environment.NewLine + "NO SE PUDO NOTIFICAR";
                }

                if (model.UsuSemestre == "ERROR")
                {
                    throw new Exception("DEBUG");
                }

                /*
                if(response.Success == 1)
                {
                    oCreatedAtActionResult = CreatedAtAction(nameof(AddData), new { id = model.IdUsuario }, model);
                }*/
            }
            catch (Exception ex)
            {
                string traceid = $"{ServerFS.GetBaseDir(true)}/logs/error_alta_{Guid.NewGuid()}.txt";

                oResponse.Message = ex.Message;

                Response<Notificacion?> notificacion = PlantillaManager.GetNotificacion
                (
                    new()
                    {
                        { "usuario", model }
                    },
                    1,PlantillaManager.FILTRO_ERROR_ALTA_USUARIO
                );

                Response<string> response = await _servicioNotificacion.EnviarAsync(notificacion.Data!);

                if(response.Success != 1)
                {
                    oResponse.Message += Environment.NewLine + "NO SE PUDO NOTIFICAR";
                }

                await System.IO.File.WriteAllTextAsync(traceid, oResponse.Message);
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
                    oUsuario.UsuNombres = model.UsuNombres.ToUpper().Trim();
                    oUsuario.UsuPrimerApellido = model.UsuPrimerApellido.ToUpper().Trim();
                    oUsuario.UsuSegundoApellido = model.UsuSegundoApellido?.ToUpper().Trim();
                    oUsuario.UsuCurp = model.UsuCurp.ToUpper().Trim();
                    oUsuario.UsuFileNameCurp = model.UsuFileNameCurp;
                    oUsuario.UsuNoCelularAnterior = model.UsuNoCelularAnterior;
                    oUsuario.UsuNoCelularActual = model.UsuNoCelularActual;
                    // DATOS ACADÉMICOS
                    oUsuario.UsuBoletaAlumnoEgresado = model.UsuBoletaAlumnoEgresado;
                    oUsuario.UsuBoletaPosgrado = model.UsuBoletaPosgrado;
                    oUsuario.UsuIdCarrera = model.UsuIdCarrera;
                    oUsuario.UsuSemestre = model.UsuSemestre;
                    oUsuario.UsuAnioEgreso = model.UsuAnioEgreso;
                    oUsuario.UsuFileNameComprobanteEstudios = model.UsuFileNameComprobanteEstudios;
                    // DATOS LABORALES
                    oUsuario.UsuNumeroEmpleadoContrato = model.UsuNumeroEmpleadoContrato;
                    oUsuario.UsuIdAreaDepto = model.UsuIdAreaDepto;
                    oUsuario.UsuNoExtensionAnterior = model.UsuNoExtensionAnterior;
                    oUsuario.UsuNoExtensionActual = model.UsuNoExtensionActual;
                    // DATOS DE LAS CREDENCIALES DE LA CUENTA EN LA APP
                    oUsuario.UsuCorreoPersonalCuentaAnterior = model.UsuCorreoPersonalCuentaAnterior;
                    oUsuario.UsuCorreoPersonalCuentaActual = model.UsuCorreoPersonalCuentaActual.Trim();
                    oUsuario.UsuContrasenia = model.UsuContrasenia;
                    oUsuario.UsuRecuperarContrasenia = false;
                    // DATOS DEL CORREO INSTITUCIONAL
                    oUsuario.UsuCorreoInstitucionalCuenta = model.UsuCorreoInstitucionalCuenta?.Trim();
                    oUsuario.UsuCorreoInstitucionalContrasenia = model.UsuCorreoInstitucionalContrasenia;
                    // OTROS DATOS
                    // oUsuario.UsuFechaHoraAlta = DateTime.UtcNow;
                    oUsuario.UsuFechaHoraActualizacion = DateTime.Now;
                    oUsuario.UsuStatus = true;
                    // DATOS FK NAVIGATION
                    oUsuario.UsuIdAreaDeptoNavigation = null;
                    oUsuario.UsuIdCarreraNavigation = null;
                    oUsuario.UsuIdRolNavigation = null!;
                    oUsuario.UsuIdTipoPersonalNavigation = null!;

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
            Response<bool> oRespuesta = new();
            try
            {
                MpTbUsuario? oUsuario = await _db.MpTbUsuarios.Where(u => u.UsuCorreoPersonalCuentaActual == correoPersonal && u.UsuCurp == curp && u.UsuStatus == true)
                                                              .FirstOrDefaultAsync();

                if (oUsuario != null)
                {
                    
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@#$%&";
                    
                    string clave = new string(Enumerable.Repeat(chars, 10).Select(s => s[new Random().Next(s.Length)]).ToArray());
                    
                    oUsuario.UsuContrasenia = Encrypt.GetSHA256(clave);
                    oUsuario.UsuRecuperarContrasenia = true;
                    oUsuario.UsuFechaHoraActualizacion = DateTime.Now;

                    _db.Entry(oUsuario).State = EntityState.Modified;

                    int cambios = await _db.SaveChangesAsync();

                    if(cambios == 1)
                    {
                        oRespuesta.Success = 1;

                        Dictionary<string, object?> datos = new()
                        {
                            {
                                "usuario", oUsuario 
                            },
                            {
                                "datos", new Dictionary<string, object?>()
                                {
                                    {"clave" , clave }
                                }
                            }
                        };
                        
                        Response<Notificacion?> notificacion = PlantillaManager.GetNotificacion(datos, 1 , PlantillaManager.FILTRO_RECUPERACION_CONTRA);

                        oRespuesta.Data = notificacion is not null && notificacion.Success == 1;

                        if (oRespuesta.Data)
                        {
                            Response<string> response = await _servicioNotificacion.EnviarAsync(notificacion!.Data!);
                            oRespuesta.Data = response.Success == 1;
                            if (!oRespuesta.Data)
                            {
                                oRespuesta.Message = response.Data!;
                            }
                        }
                        else
                        {
                            oRespuesta.Message = notificacion!.Message;
                        }
                    }


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
                
                //PlantillaManager plantillaManager = new PlantillaManager(plantillas);
                //McCatPlantillas plantillas1 = plantillaManager.GetPlantilla()

                if (oUsuario != null)
                {
                    oUsuario.UsuContrasenia = Encrypt.GetSHA256(newPassword);
                    oUsuario.UsuRecuperarContrasenia = false;
                    oUsuario.UsuFechaHoraActualizacion = DateTime.Now;

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
                // db.Remove(oPersona);

                if (oUsuario != null)
                {
                    oUsuario.UsuStatus = isActivate;
                    oUsuario.UsuFechaHoraActualizacion = DateTime.Now;

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
        } // editByIdStatus

        [HttpGet("generar_random/{cantidad}")]
        public async Task<IActionResult> GenerarUsuarios(int cantidad = 10)
        {
            Response<string> oResponse = new() { Data = string.Empty };

            Random rnd = new();
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
                    new (10,    TipoPersonal.POSGRADO),
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
                        UsuFileNameComprobanteEstudios = "-",
                        UsuNoExtensionActual = "0",
                        UsuBoletaPosgrado = "B000000",
                        UsuNumeroEmpleadoContrato = "0",
                        UsuIdAreaDepto = 1,
                        UsuIdCarrera = 8,
                        UsuStatus = true
                    };

                    tmp.UsuNombres = es_hombre ? nombres_h.GetRandomItem() : nombres_m.GetRandomItem();

                    if (tiene_varios_nombres)
                    {
                        tmp.UsuNombres += " " + (es_hombre ? nombres_h.GetRandomItem() : nombres_m.GetRandomItem());
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

                        case TipoPersonal.POSGRADO:
                            es_alumno = true;
                            anios_estudio = rnd.Next(4, 6);
                            anios_egreso = rnd.Next(0, 20);

                            tmp.UsuAnioEgreso = DateTime.Now.Year - (anios_egreso + anios_estudio);
                            break;
                    }

                    anio_nacimiento = DateTime.Now.Year - (18 + anios_egreso + anios_estudio);

                    tmp.UsuBoletaAlumnoEgresado = $"{DateTime.Now.Year - anios_estudio - anios_egreso}600{tmp.UsuNombres[0] % 10}{tmp.UsuPrimerApellido[0] % 10}{(tmp.UsuSegundoApellido ?? "X")[0] % 10}";

                    fecha_nacimiento = new DateOnly(anio_nacimiento, rnd.Next(1, 12), 1);

                    tmp.UsuCurp = CURP.Generar(tmp.UsuNombres, tmp.UsuPrimerApellido, tmp.UsuSegundoApellido, fecha_nacimiento, es_hombre?'H':'M', estados.GetRandomItem());

                    tmp.UsuFileNameCurp = $"CURP_{id}.pdf";

                    if (es_alumno)
                    {
                        tmp.UsuIdCarrera = rnd.Next(1, 7);
                        tmp.UsuFileNameComprobanteEstudios = $"COMPROBANTE_INSCRIPCION_{id}.pdf";
                    }

                    tmp.UsuContrasenia = Encrypt.GetSHA256(tmp.UsuCurp);
                    tmp.UsuCorreoPersonalCuentaActual = "noreply@localhost";
                    tmp.UsuNoCelularActual = "55 00 00 00 00";

                    //await _db.MpTbUsuarios.AddAsync(tmp);
                    registros.Add(tmp);

                }//FOR

                await _db.MpTbUsuarios.AddRangeAsync(registros);
                await _db.SaveChangesAsync();

                oResponse.Success = 1;
            }
            catch (Exception ex)
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
