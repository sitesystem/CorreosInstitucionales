using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.tbUsuarios
{
    public interface IUsuarioService : IGenericService<RequestDTO_Usuario>
    {
        public Task<Response<RequestDTO_Usuario>?> ValidateByEmailCURP(string correo, string curp);
        public Task<HttpResponseMessage> ResetPassword(string correoPersonal, string curp);
        public Task<HttpResponseMessage> ChangePassword(int id, string newPassword);
    }
}
