namespace CorreosInstitucionales.Client.CapaPresentation.ComponentsPages.UI_UX.Login
{
    public interface ILoginServices
    {
        Task Login(string token);
        Task Logout();
    }
}
