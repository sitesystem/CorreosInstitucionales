using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.tbUsuarios
{
    public interface IUsuarioService
    {
        public Task<Response<List<RequestDTO_Usuario>>?> GetAllDataAsync(bool filterByStatus);
        public Task<Response<RequestDTO_Usuario>?> GetDataByIdAsync(int id);
        public Task<Response<RequestDTO_Usuario>?> ValidateByEmailCURP(string correo, string curp);
        public Task<HttpResponseMessage> AddDataAsync(RequestDTO_Usuario oUsuario);
        public Task<HttpResponseMessage> EditDataAsync(RequestDTO_Usuario oUsuario);
        public Task<HttpResponseMessage> ResetPassword(string correoPersonal);
        public Task<HttpResponseMessage> ChangePassword(int id, string newPassword);
        public Task<HttpResponseMessage> EnableDisableDataByIdAsync(int id, bool isActivate);
    }
}
