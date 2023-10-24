using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catCarrerasService
{
    public interface ICarrera
    {
        public Task<Response<List<CarreraViewModel>>?> GetAllDataAsync(bool filterByStatus);

        public Task<Response<CarreraViewModel>?> GetDataByAsync(int id);

        public Task<HttpResponseMessage> AddDataAsync(CarreraViewModel oEdificio);

        public Task<HttpResponseMessage> EditDataAsync(CarreraViewModel oEdificio);

        public Task<HttpResponseMessage> EnableDisableDataById(int id, bool isActivate);
    }
}
