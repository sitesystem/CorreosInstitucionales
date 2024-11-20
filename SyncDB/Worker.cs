using CorreosInstitucionales.Shared.CapaDataAccess.DBContext;
using CorreosInstitucionales.Shared.CapaDataAccess.DBContextCentral;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SyncDB.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncDB
{
    public class Worker : IDisposable
    {
        protected readonly DBSACI _db_saci;
        protected readonly DBCentral _db_central;
        public bool IsRunning { get; private set; } = true;

        public Worker()
        {
            /* ================ CONFIGURACIÓN ================*/
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration config = builder.Build();

            /* ================ CONEXIÓN A BD ================*/

            _db_saci = new DBSACI(config.GetConnectionString("SQLServer_Connection")!);
            _db_central = new DBCentral(config.GetConnectionString("Docker_Connection")!);
        }

        public async Task RunAsync()
        {
            int max_id_usuario_saci = await _db_saci.MpTbUsuarios.MaxAsync(u => u.IdUsuario);
            int max_id_usuario_central = 0;
            int delta_usuarios = 0;

            if (_db_central.TbUsuarios.FirstOrDefault() is not null)
            {
                max_id_usuario_central = await _db_central.TbUsuarios.MaxAsync(u => u.IdUsuario);
            }

            List<MpTbUsuario> usuarios_nuevos = await _db_saci.MpTbUsuarios
                .Where(u => u.IdUsuario > max_id_usuario_central)
                .ToListAsync();

            delta_usuarios = usuarios_nuevos.Count;

            if(delta_usuarios == 0)
            {
                //Console.WriteLine("NO HAY REGISTROS PARA ACTUALIZAR.");
                return;
            }

            /* ================ SINCRONIZACIÓN ================*/
            Console.WriteLine($"================================");
            Console.WriteLine($"FASE 1 AGREGAR USUARIOS NUEVOS");
            Console.WriteLine($"================================");

            Console.WriteLine($"USUARIOS EN DB CENTRAL\t{max_id_usuario_central}");
            Console.WriteLine($"USUARIOS EN DB SACI\t{max_id_usuario_saci}");
            Console.WriteLine($"NUEVOS USUARIOS\t\t{delta_usuarios}");

            Console.WriteLine($">> INICIANDO TRANSACCIÓN");
            using (var transaction = _db_central.Database.BeginTransaction())
            {

                foreach (MpTbUsuario nuevo in usuarios_nuevos)
                {
                    await _db_central.TbUsuarios.AddAsync(new TbUsuario()
                    {
                        IdUsuario = nuevo.IdUsuario,
                        UsuNombres = nuevo.UsuNombres,
                        UsuPrimerApellido = nuevo.UsuPrimerApellido,
                        UsuSegundoApellido = nuevo.UsuSegundoApellido,

                        UsuCurp = nuevo.UsuCurp,

                        UsuNoCelularActual = nuevo.UsuNoCelularActual,
                        UsuNoCelularAnterior = nuevo.UsuNoCelularAnterior,

                        UsuCorreoPersonalActual = nuevo.UsuCorreoPersonalCuentaActual,
                        UsuCorreoPersonalAnterior = nuevo.UsuCorreoPersonalCuentaAnterior,
                        UsuCorreoInstitucional = nuevo.UsuCorreoInstitucionalCuenta,

                        UsuContraseniaPlataformas = nuevo.UsuContrasenia,
                        UsuContraseniaCorreoInstitucional = nuevo.UsuCorreoInstitucionalContrasenia,

                        UsuFechaHoraAlta = nuevo.UsuFechaHoraAlta,
                        UsuFechaHoraActualizacion = nuevo.UsuFechaHoraActualizacion,

                        UsuRecuperacionContrasenia = nuevo.UsuRecuperarContrasenia,
                        UsuStatus = nuevo.UsuStatus
                    });
                    Console.WriteLine($"AGREGADO {nuevo.IdUsuario}");
                }

                try
                {
                    await _db_central.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT TbUsuarios ON");
                    await _db_central.SaveChangesAsync();
                    await _db_central.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT TbUsuarios OFF");
                    transaction.Commit();
                    Console.WriteLine($"TRANSACCIÓN FINALIZADA <<");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException!.Message);
                    Console.WriteLine(ex.InnerException!.StackTrace);

                    transaction.Rollback();
                    Console.WriteLine($"TRANSACCIÓN CANCELADA");
                }

            }
            Console.WriteLine($"================================");
        }

        public void Dispose()
        {
            _db_saci.Dispose();
            _db_central.Dispose();
        }
    }
}
