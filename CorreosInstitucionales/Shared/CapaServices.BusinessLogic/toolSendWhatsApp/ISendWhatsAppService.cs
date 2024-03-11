using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaEntities.Request;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolSendWhatsApp
{
    public interface ISendWhatsAppService
    {
        public Task<HttpResponseMessage> SendWhatsAppAsync(RequestDTO_SendWhatsApp oSendWhatsApp);
    }
}
