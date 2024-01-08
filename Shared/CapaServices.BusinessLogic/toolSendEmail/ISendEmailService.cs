using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaEntities.Request;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolSendEmail
{
    public interface ISendEmailService
    {
        public Task<HttpResponseMessage> SendEmailAsync(RequestDTO_SendEmail oSendEmail);
    }
}
