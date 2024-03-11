using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.Request
{
    public class RequestDTO_SendWhatsApp
    {
        public string Number { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
