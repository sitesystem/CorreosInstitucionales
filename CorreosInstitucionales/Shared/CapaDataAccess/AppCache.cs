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
        public static McCatEscuela? Escuela = new McCatEscuela();
        
        public static McCatLink[] Enlaces = [];
        public static McCatPlantillas[] Plantillas = [];
        public static McCatSemestre[] Semestres = [];

        public static List<McCatAnuncio> Anuncios = [];

        public static byte[] LogoSACI = [];
    }
}
