using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request
{
    public class SolicitudViewModel
    {
        public int IdSolicitud { get; set; }

        public int SolIdTipoSolicitud { get; set; }

        public int SolIdEstadosSolicitud { get; set; }

        public int? SolIdAreaDepto { get; set; }

        public int SolIdUsuario { get; set; }

        public DateTime SolFecha { get; set; }
    }
}
