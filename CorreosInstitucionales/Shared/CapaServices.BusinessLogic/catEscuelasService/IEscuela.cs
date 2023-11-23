using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catEscuelasService
{
    public interface IEscuela
    {
        public Task<Response<List<EscuelaViewModel>>?> GetAllDataAsync(bool filterByStatus);

        public Task<Response<EscuelaViewModel>?> GetDataByAsync(int id);

        public Task<HttpResponseMessage> AddDataAsync(EscuelaViewModel oEscuela);

        public Task<HttpResponseMessage> EditDataAsync(EscuelaViewModel oEscuela);

        public Task<HttpResponseMessage> EnableDisableDataById(int id, bool isActivate);
    }
}
