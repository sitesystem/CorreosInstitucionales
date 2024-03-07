using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.Common
{
    public class RegistroImportacion
    {
        public string CURP { get; set; } = string.Empty;
        public string ID { get; set; } = string.Empty ;
        public string NoExtension {  get; set; } = string.Empty ; 
        public string CorreoPersonal {  get; set; } = string.Empty ;
        public string CorreoInstitucional { get; set; } = string.Empty;
        public string Clave {  get; set; } = string.Empty ;
        public string Accion { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{CURP}|{ID}|{NoExtension}|{CorreoPersonal}|{CorreoInstitucional}|{Clave}|{Accion}";
        }
    }
}
