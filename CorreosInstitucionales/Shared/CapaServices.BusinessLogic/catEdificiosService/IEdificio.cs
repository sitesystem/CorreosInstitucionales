using siti.Shared.CapaEntities.ViewModels.Request.MóduloCatálogos;
using siti.Shared.CapaEntities.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace siti.Shared.CapaServices.BusinessLogic.MóduloCatálogos.catEdificiosService
{
    public interface IEdificio
    {
        public Task<Response<List<EdificioViewModel>>?> GetAllDataAsync(bool filterByStatus);
        public Task<Response<EdificioViewModel>?> GetDataByIdAsync(int id);
        public Task<HttpResponseMessage> AddDataAsync(EdificioViewModel oEdificio);
        public Task<HttpResponseMessage> EditDataAsync(EdificioViewModel oEdificio);
        public Task<HttpResponseMessage> EnableDisableDataById(int id, bool isActivate);
    }
}
