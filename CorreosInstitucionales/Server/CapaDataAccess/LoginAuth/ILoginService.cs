using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request.Login;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;

namespace CorreosInstitucionales.Server.CapaDataAccess.LoginAuth
{
    public interface ILoginService
    {
        LoginUsuarioResponse Auth(LoginAuthViewModel model);
    }
}
