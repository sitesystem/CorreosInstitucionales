using CorreosInstitucionales.Shared.CapaEntities.Request;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers
{
    public interface IGenericService<TGet, TAddEdit>
    {
        public Task<List<TGet>?> GetAllData(bool filterByStatus);
        public Task<TGet?> GetDataById(int id);
        public Task AddData(TAddEdit model);
        public Task EditData(TAddEdit model);
        public Task EnableDisableDataById(int id, bool isActivate);
    }
}
