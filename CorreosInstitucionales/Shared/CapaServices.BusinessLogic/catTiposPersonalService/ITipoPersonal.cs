using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catTiposPersonalService
{
    public interface ITipoPersonal
    {
        public Task<Response<List<TipoPersonalViewModel>>?> GetAllDataAsync(bool filterByStatus);

        public Task<Response<TipoPersonalViewModel>?> GetDataByAsync(int id);

        public Task<HttpResponseMessage> AddDataAsync(TipoPersonalViewModel oTipoPersonal);

        public Task<HttpResponseMessage> EditDataAsync(TipoPersonalViewModel oTipoPersonal);

        public Task<HttpResponseMessage> EnableDisableDataById(int id, bool isActivate);
    }
}
