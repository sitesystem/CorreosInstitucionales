using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catNoExtensiones
{
    public interface INoExtensionService
    {
        public Task<Response<List<RequestViewModel_NoExtension>>?> GetAllDataAsync(bool filterByStatus);
        public Task<Response<RequestViewModel_NoExtension>?> GetDataByIdAsync(int id);
        public Task<HttpResponseMessage> AddDataAsync(RequestViewModel_NoExtension oNoExtension);
        public Task<HttpResponseMessage> EditDataAsync(RequestViewModel_NoExtension oNoExtension);
        public Task<HttpResponseMessage> EnableDisableDataByIdAsync(int id, bool isActivate);
    }
}
