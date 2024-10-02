using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.Common
{
    public class RegistroImportacion
    {
        public string Ticket { get; set; } = string.Empty;
        public string CURP { get; set; } = string.Empty;
        public string ID { get; set; } = string.Empty ;
        public string NoExtension {  get; set; } = string.Empty ;
        public string Celular { get; set; } = string.Empty;
        public string CorreoPersonal {  get; set; } = string.Empty ;
        public string CorreoInstitucional { get; set; } = string.Empty;
        public string Clave {  get; set; } = string.Empty ;
        public string Accion { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{Ticket}|{CURP}|{ID}|{NoExtension}|{Celular}|{CorreoPersonal}|{CorreoInstitucional}|{Clave}|{Accion}";
        }

        public static string GetHeaders()
        {
            return "Ticket|CURP|ID|NoExtension|Celular|CorreoPersonal|CorreoInstitucional|Clave|Accion";
        }
    }
}
