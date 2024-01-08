using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catAreasDeptos
{
    public interface IAreaDeptoService
    {
        public Task<Response<List<RequestViewModel_AreaDepto>>?> GetAllDataAsync(bool filterByStatus);
        public Task<Response<RequestViewModel_AreaDepto>?> GetDataByIdAsync(int id);
        public Task<HttpResponseMessage> AddDataAsync(RequestViewModel_AreaDepto oAreaDepto);
        public Task<HttpResponseMessage> EditDataAsync(RequestViewModel_AreaDepto oAreaDepto);
        public Task<HttpResponseMessage> EnableDisableDataById(int id, bool isActivate);
    }
}
