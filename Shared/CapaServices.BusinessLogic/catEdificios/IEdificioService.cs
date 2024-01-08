using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catEdificios
{
    public interface IEdificioService
    {
        public Task<Response<List<RequestViewModel_Edificio>>?> GetAllDataAsync(bool filterByStatus);
        public Task<Response<RequestViewModel_Edificio>?> GetDataByIdAsync(int id);
        public Task<HttpResponseMessage> AddDataAsync(RequestViewModel_Edificio oEdificio);
        public Task<HttpResponseMessage> EditDataAsync(RequestViewModel_Edificio oEdificio);
        public Task<HttpResponseMessage> EnableDisableDataById(int id, bool isActivate);
    }
}
