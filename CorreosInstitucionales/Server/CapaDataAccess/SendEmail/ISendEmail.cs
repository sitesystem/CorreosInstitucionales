using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;

namespace CorreosInstitucionales.Server.CapaDataAccess.SendEmail
{
    public interface ISendEmail
    {
        public void SendEmail(SendEmailViewModel oEmailTo);
    }
}
