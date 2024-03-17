using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.Constantes
{
    public enum TipoDocumento
    {
        CURP            = 0,
        COM_INSCRIPCION = 1,
        CAP_ANTIVIRUS   = 2,
        CAP_BLOQUEO     = 3,
        CAP_ERROR       = 4,
    }

    public static class TipoDocumentoMethods
    {
        public static string GetNombre(this TipoDocumento documento)
        {
            switch(documento)
            {
                case TipoDocumento.CURP:            return "CURP";
                case TipoDocumento.COM_INSCRIPCION: return "COMPROBANTE_INSCRIPCION";
                case TipoDocumento.CAP_ANTIVIRUS:   return "CAPTURA_ANTIVIRUS";
                case TipoDocumento.CAP_BLOQUEO:     return "CAPTURA_BLOQUEO";
                case TipoDocumento.CAP_ERROR:       return "CAPTURA_ERROR";
            }

            return "ARCHIVO";
        }
    }
}
