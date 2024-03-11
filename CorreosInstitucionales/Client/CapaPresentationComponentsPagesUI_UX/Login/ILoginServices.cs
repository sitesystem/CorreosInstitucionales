namespace CorreosInstitucionales.Client.CapaPresentationComponentsPagesUI_UX.Login
{
    public interface ILoginServices
    {
        Task Login(string token);
        Task Logout();
    }
}
