using CorreosInstitucionales.Shared.CapaEntities.Request;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers.SendEmail
{
    public interface ISendEmailService
    {
        public Task SendEmail(RequestDTO_SendEmail oEmailTo);
    }
}
