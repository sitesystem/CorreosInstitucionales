using CorreosInstitucionales.Shared.CapaDataAccess.DBContext;
using CorreosInstitucionales.Shared.CapaTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaDataAccess
{
    public static class AppCache
    {
        public static string NombreEscuela = string.Empty;
        public static string NombreEscuela_Corto = string.Empty;
        public static int IdEscuela = 0;
        
        public static McCatLink[] Enlaces = [];
        public static McCatPlantillas[] Plantillas = [];

        public static List<McCatAnuncio> Anuncios = [];

        public static byte[] LogoSACI = [];
    }
}
