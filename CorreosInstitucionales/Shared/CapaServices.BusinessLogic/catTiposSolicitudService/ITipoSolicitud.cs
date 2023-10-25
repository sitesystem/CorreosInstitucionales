using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catTiposSolicitudService
{
    public interface ITipoSolicitud
    {
        public Task<Response<List<TipoSolicitudViewModel>>?> GetAllDataAsync(bool filterByStatus);

        public Task<Response<TipoSolicitudViewModel>?> GetDataByIdAsync(int id);

        public Task<HttpResponseMessage> AddDataAsync(TipoSolicitudViewModel oTipoSolicitud);

        public Task<HttpResponseMessage> EditDataAsync(TipoSolicitudViewModel oTipoSolicitud);

        public Task<HttpResponseMessage> EnableDisableDataById(int id, bool isActivate);
    }
}
