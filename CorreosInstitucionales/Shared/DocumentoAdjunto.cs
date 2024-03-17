using CorreosInstitucionales.Shared.Constantes;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared
{
    public class DocumentoAdjunto<T>
    {
        public T Value { get; set; }
        public Dictionary<TipoDocumento, IBrowserFile?>  Files { get; set; }

        public DocumentoAdjunto(T v, IBrowserFile? f, TipoDocumento t)
        {
            Value = v;
            Files = new()
            {
                { t, f }
            };
        }

        public DocumentoAdjunto(T v, Dictionary<TipoDocumento, IBrowserFile?> files)
        {
            Value = v;
            Files = files;
        }
    }
}
