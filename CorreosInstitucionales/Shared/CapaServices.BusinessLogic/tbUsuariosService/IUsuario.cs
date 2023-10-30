using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.tbUsuariosService
{
    public interface IUsuario
    {
        public Task<Response<List<UsuarioViewModel>>?> GetAllDataAsync(bool filterByStatus);

        public Task<Response<UsuarioViewModel>?> GetDataByAsync(int id);

        public Task<HttpResponseMessage> AddDataAsync(UsuarioViewModel oUsuario);

        public Task<HttpResponseMessage> EditDataAsync(UsuarioViewModel oUsuario);

        public Task<HttpResponseMessage> EnableDisableDataById(int id, bool isActivate);
    }
}
