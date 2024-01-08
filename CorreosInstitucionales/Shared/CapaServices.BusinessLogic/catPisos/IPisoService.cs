using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catPisos
{
    public interface IPisoService
    {
        public Task<Response<List<RequestViewModel_Piso>>?> GetAllDataAsync(bool filterByStatus);
        public Task<Response<RequestViewModel_Piso>?> GetDataByIdAsync(int id);
        public Task<HttpResponseMessage> AddDataAsync(RequestViewModel_Piso oPiso);
        public Task<HttpResponseMessage> EditDataAsync(RequestViewModel_Piso oPiso);
        public Task<HttpResponseMessage> EnableDisableDataByIdAsync(int id, bool isActivate);
    }
}
