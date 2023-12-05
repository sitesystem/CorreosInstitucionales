using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catAreasDeptosService
{
    public interface IAreaDepto
    {
        public Task<Response<List<AreaDeptoViewModel>>?> GetAllDataAsync(bool filterByStatus);

        public Task<Response<AreaDeptoViewModel>?> GetDataByAsync(int id);

        public Task<HttpResponseMessage> AddDataAsync(AreaDeptoViewModel oAreaDepto);

        public Task<HttpResponseMessage> EditDataAsync(AreaDeptoViewModel oAreaDepto);

        public Task<HttpResponseMessage> EnableDisableDataById(int id, bool isActivate);
    }
}
