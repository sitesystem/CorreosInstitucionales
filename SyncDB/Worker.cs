using CorreosInstitucionales.Shared.CapaDataAccess;
using CorreosInstitucionales.Shared.CapaDataAccess.DBContext;
using CorreosInstitucionales.Shared.CapaDataAccess.DBContextCentral;
using CorreosInstitucionales.Shared.CapaTools;
using CorreosInstitucionales.Shared.Constantes;
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

        public Worker(IConfiguration config)
        {
            /* ================ CONEXIÓN A BD ================*/

            _db_saci = new DBSACI(config.GetConnectionString("SQLServer_Connection")!);
            _db_central = new DBCentral(config.GetConnectionString("Docker_Connection")!);
        }

        public async Task RunAsync()
        {
            await Syncronize();
            await EncuestaCalidad();
        }

        public async Task EncuestaCalidad()
        {
            int id_pendinte = (int)TipoEstadoSolicitud.ATENDIDA;

            MtTbSolicitudesTicket[] encuestas = await _db_saci.MtTbSolicitudesTickets
                .Where(
                    s => 
                        s.SolEncuestaCalidadCalificacion == null &&
                        s.SolIdEstadoSolicitud == id_pendinte
                )
                .Include(u => u.SolIdUsuarioNavigation)
                .ToArrayAsync();

            Console.WriteLine($"ENCUESTAS PENDIENTES: {encuestas.Length}");

            foreach(MtTbSolicitudesTicket encuesta in encuestas)
            {
                Console.WriteLine($"{encuesta.IdSolicitudTicket} - {encuesta.SolEnvioEncuesta}");   
            }
        }

        public async Task Syncronize()
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

            if (delta_usuarios == 0)
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
                    await _db_central.TbUsuarios.AddAsync(EntityUtils.Convertir_UsuarioCentral(nuevo));
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
            //Console.WriteLine($"================================");
        }

        public void Dispose()
        {
            _db_saci.Dispose();
            _db_central.Dispose();
        }
    }
}
