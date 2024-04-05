using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaEntities.Response;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic
{
    public interface IGenericService<T>
    {
        public Task<Response<List<T>>?> GetAllDataByStatusAsync(bool filterByStatus);
        public Task<Response<T>?> GetDataByIdAsync(int? id);
        public Task<HttpResponseMessage> AddDataAsync(T oObject);
        public Task<HttpResponseMessage> EditDataAsync(T oObject);
        public Task<HttpResponseMessage> EnableDisableDataByIdAsync(int id, bool isActivate);
    }
}
