using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic
{
    public interface IGenericService<T>
    {
        public Task<Response<List<T>>?> GetAllDataAsync(bool filterByStatus);
        public Task<Response<T>?> GetDataByIdAsync(int? id);
        public Task<HttpResponseMessage> AddDataAsync(T oAreaDepto);
        public Task<HttpResponseMessage> EditDataAsync(T oAreaDepto);
        public Task<HttpResponseMessage> EnableDisableDataByIdAsync(int id, bool isActivate);
    }
}
