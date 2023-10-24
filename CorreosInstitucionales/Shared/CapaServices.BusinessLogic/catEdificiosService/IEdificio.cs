using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catEdificiosService
{
    public interface IEdificio
    {
        public Task<Response<List<EdificioViewModel>>?> GetAllDataAsync(bool filterByStatus);

        public Task<Response<EdificioViewModel>?> GetDataByAsync(int id);

        public Task<HttpResponseMessage> AddDataAsync(EdificioViewModel oEdificio);

        public Task<HttpResponseMessage> EditDataAsync(EdificioViewModel oEdificio);

        public Task<HttpResponseMessage> EnableDisableDataById(int id, bool isActivate);
    }
}
