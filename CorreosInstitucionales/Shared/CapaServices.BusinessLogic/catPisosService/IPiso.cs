using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catPisosService
{
    public interface IPiso
    {
        public Task<Response<List<PisoViewModel>>?> GetAllDataAsync(bool filterByStatus);

        public Task<Response<PisoViewModel>?> GetDataByAsync(int id);

        public Task<HttpResponseMessage> AddDataAsync(PisoViewModel oPiso);

        public Task<HttpResponseMessage> EditDataAsync(PisoViewModel oPiso);

        public Task<HttpResponseMessage> EnableDisableDataById(int id,  bool isActivate);

    }
}
