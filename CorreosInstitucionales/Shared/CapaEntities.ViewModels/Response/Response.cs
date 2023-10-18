using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response
{
    public class Response<T>
    {
        public int Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        
        public Response()
        {
            Success = 0;
        }
    }
}
