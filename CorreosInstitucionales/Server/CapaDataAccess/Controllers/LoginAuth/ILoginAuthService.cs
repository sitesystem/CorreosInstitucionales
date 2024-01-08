using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers.LoginAuth
{
    public interface ILoginAuthService
    {
        public Task<ResponseDTO_LoginUsuario> Auth(RequestDTO_LoginAuth model);
    }
}
