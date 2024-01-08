using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catEscuelas
{
    public interface IEscuelaService
    {
        public Task<Response<List<RequestViewModel_Escuela>>?> GetAllDataAsync(bool filterByStatus);
        public Task<Response<RequestViewModel_Escuela>?> GetDataByIdAsync(int id);
        public Task<HttpResponseMessage> AddDataAsync(RequestViewModel_Escuela oEscuela);
        public Task<HttpResponseMessage> EditDataAsync(RequestViewModel_Escuela oEscuela);
        public Task<HttpResponseMessage> EnableDisableDataByIdAsync(int id, bool isActivate);
    }
}
