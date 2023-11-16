using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catLinksService
{
    public interface ILink
    {
        public Task<Response<List<LinkViewModel>>?> GetAllDataAsync(bool filterByStatus);

        public Task<Response<LinkViewModel>?> GetDataByIdAsync(int id);

        public Task<HttpResponseMessage> AddDataAsync(LinkViewModel oLink);

        public Task<HttpResponseMessage> EditDataAsync(LinkViewModel oLink);

        public Task<HttpResponseMessage> EnableDisableDataById(int id, bool isActivate);
    }
}
