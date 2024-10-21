using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaDataAccess.DBContext;

namespace CorreosInstitucionales.Shared.CapaEntities.Request
{
    public class RequestViewModel_EstadoSolicitud : McCatEstadosSolicitud
    {
        /// <summary>
        /// Estado de la Solicitud (1 - Levantado, 2 - Pendiente, 3 - En Proceso, 4 - Atendido, 5 - Encuesta Contestada, 6 - Cancelada)
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo ESTADO requerido.")]
        public new string EdosolNombreEstado
        {
            get { return base.EdosolNombreEstado; }
            set { base.EdosolNombreEstado = value; }
        }
    }
}
