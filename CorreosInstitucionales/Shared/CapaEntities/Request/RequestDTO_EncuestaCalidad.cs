using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.Request
{
    public class RequestDTO_EncuestaCalidad
    {
        public int? IdSolicitud { get; set; }

        [Range(1, 5, ErrorMessage = "Campo CALIFICACIÓN DE LA ENCUESTA requerido.")]
        public int Calificacion { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo OBSERVACIONES / COMENTARIOS requerido.")]
        public string Comentarios { get; set; } = null!;
    }
}
