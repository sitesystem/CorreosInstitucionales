using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request
{
    public class EstadoSolicitudViewModel
    {
        [Key]
        public int IdEstadoSolicitud { get; set; }

        [Column("edosolNombreEstado")]
        [StringLength(50)]
        public string EdosolNombreEstado { get; set; } = null!;

        [Column("edosolDescripcion", TypeName = "text")]
        public string? EdosolDescripcion { get; set; }

        //[InverseProperty("SolIdEstadoSolicitudNavigation")]
        //public virtual ICollection<MceTbSolicitudTicket> MceTbSolicitudTickets { get; set; } = new List<MceTbSolicitudTicket>();
    }
}
