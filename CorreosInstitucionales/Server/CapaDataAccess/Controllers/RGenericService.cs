using CorreosInstitucionales.Shared.CapaEntities.Request;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers
{
    public class RGenericService(DbCorreosInstitucionalesUpiicsaContext db) : IGenericService<McCatAreasDepto, RequestViewModel_AreaDepto>
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;

        public async Task<List<McCatAreasDepto>?> GetAllData(bool filterByStatus)
        {
            var list = new List<McCatAreasDepto>();

            if (filterByStatus)
                list = await _db.McCatAreasDeptos
                                .Where(a => a.AreStatus.Equals(filterByStatus))
                                .Include(a => a.AreIdEdificioNavigation)
                                .Include(a => a.AreIdPisoNavigation)
                                .ToListAsync();
            else
                list = await _db.McCatAreasDeptos
                                .Include(a => a.AreIdEdificioNavigation)
                                .Include(a => a.AreIdPisoNavigation)
                                .ToListAsync();
            return list;
        }

        public async Task<McCatAreasDepto?> GetDataById(int id)
        {
            var item = new McCatAreasDepto();

            item = await _db.McCatAreasDeptos
                            .Include(a => a.AreIdEdificioNavigation)
                            .Include(a => a.AreIdPisoNavigation)
                            .FirstOrDefaultAsync(a => a.IdAreaDepto == id);
            return item;
        }

        public async Task AddData(RequestViewModel_AreaDepto model)
        {
            McCatAreasDepto oÁreaDepto = new()
            {
                IdAreaDepto = model.IdAreaDepto,
                AreNombreAreaDepto = model.AreNombreAreaDepto,
                AreTitular = model.AreTitular,
                AreIdEdificio = model.AreIdEdificio,
                AreIdPiso = model.AreIdPiso,
                AreStatus = true,
                AreIdEdificioNavigation = null,
                AreIdPisoNavigation = null,
            };

            await _db.McCatAreasDeptos.AddAsync(oÁreaDepto);
            await _db.SaveChangesAsync();
        }

        public async Task EditData(RequestViewModel_AreaDepto model)
        {
            McCatAreasDepto? oÁreaDepto = await _db.McCatAreasDeptos.FindAsync(model.IdAreaDepto);

            if (oÁreaDepto != null)
            {
                oÁreaDepto.AreNombreAreaDepto = model.AreNombreAreaDepto;
                oÁreaDepto.AreTitular = model.AreTitular;
                oÁreaDepto.AreIdEdificio = model.AreIdEdificio;
                oÁreaDepto.AreIdPiso = model.AreIdPiso;
                oÁreaDepto.AreStatus = model.AreStatus;

                _db.Entry(oÁreaDepto).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
        }

        public async Task EnableDisableDataById(int id, bool isActivate)
        {
            McCatAreasDepto? oÁreaDepto = await _db.McCatAreasDeptos.FindAsync(id);

            if (oÁreaDepto != null)
            {
                oÁreaDepto.AreStatus = isActivate;
                _db.Entry(oÁreaDepto).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
        }
    }
}
