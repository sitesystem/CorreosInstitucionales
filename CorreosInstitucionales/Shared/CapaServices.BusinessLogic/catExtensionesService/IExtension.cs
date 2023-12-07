using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catExtensionService
{
    public interface IExtension
    {
        public Task<Response<List<ExtensionViewModel>>?> GetAllDataAsync(bool filterByStatus);
        public Task<Response<ExtensionViewModel>?> GetDataByIdAsync(int id);
        public Task<HttpResponseMessage> AddDataAsync(ExtensionViewModel oLink);
        public Task<HttpResponseMessage> EditDataAsync(ExtensionViewModel oLink);
        public Task<HttpResponseMessage> EnableDisableDataByIdAsync(int id, bool isActivate);
    }
}
