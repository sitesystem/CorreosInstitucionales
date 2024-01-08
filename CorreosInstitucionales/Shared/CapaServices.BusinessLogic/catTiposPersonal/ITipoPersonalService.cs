using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catTiposPersonal
{
    public interface ITipoPersonalService
    {
        public Task<Response<List<RequestViewModel_TipoPersonal>>?> GetAllDataAsync(bool filterByStatus);
        public Task<Response<RequestViewModel_TipoPersonal>?> GetDataByIdAsync(int id);
        public Task<HttpResponseMessage> AddDataAsync(RequestViewModel_TipoPersonal oTipoPersonal);
        public Task<HttpResponseMessage> EditDataAsync(RequestViewModel_TipoPersonal oTipoPersonal);
        public Task<HttpResponseMessage> EnableDisableDataByIdAsync(int id, bool isActivate);
    }
}
