using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catLinks
{
    public interface ILinkService
    {
        public Task<Response<List<RequestViewModel_Link>>?> GetAllDataAsync(bool filterByStatus);
        public Task<Response<RequestViewModel_Link>?> GetDataByIdAsync(int id);
        public Task<Response<RequestViewModel_Link>?> GetDataByNameAsync(string name);
        public Task<HttpResponseMessage> AddDataAsync(RequestViewModel_Link oLink);
        public Task<HttpResponseMessage> EditDataAsync(RequestViewModel_Link oLink);
        public Task<HttpResponseMessage> EnableDisableDataByIdAsync(int id, bool isActivate);
    }
}
