using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.SendEmailService
{
    public interface ISendEmail
    {
        public Task<HttpResponseMessage> SendEmailAsync(SendEmailViewModel oSendEmail);
    }
}
