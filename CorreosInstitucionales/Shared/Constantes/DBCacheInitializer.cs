using CorreosInstitucionales.Shared.CapaDataAccess.DBContext;
using CorreosInstitucionales.Shared.CapaTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.Constantes
{
    public class DBCacheInitializer (DbCorreosInstitucionalesUpiicsaContext context)
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _context = context;

        public void Init()
        {
            PlantillaManager.Plantillas = _context.McCatPlantillas.Where(p => p.PlaStatus).ToArray();
        }
    }
}
