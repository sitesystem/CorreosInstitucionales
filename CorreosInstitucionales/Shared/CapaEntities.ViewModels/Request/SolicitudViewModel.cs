using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request
{
    public class SolicitudViewModel
    {
        [Key]
        public int IdSolicitudTicket { get; set; }

        [Column("solIdTipoSolicitud")]
        public int SolIdTipoSolicitud { get; set; }

        [Column("solIdUsuario")]
        public int SolIdUsuario { get; set; }

        [Column("solCapturaEscaneoAntivirus")]
        [StringLength(150)]
        public string? SolCapturaEscaneoAntivirus { get; set; }

        [Column("solCapturaCuentaBloqueada")]
        [StringLength(150)]
        public string? SolCapturaCuentaBloqueada { get; set; }

        [Column("solCapturaError")]
        [StringLength(150)]
        public string? SolCapturaError { get; set; }

        [Column("solFechaHoraCreacion", TypeName = "datetime")]
        public DateTime SolFechaHoraCreacion { get; set; }

        [Column("solIdEstadoSolicitud")]
        public int SolIdEstadoSolicitud { get; set; }

        [ForeignKey("SolIdEstadoSolicitud")]
        [InverseProperty("MceTbSolicitudTickets")]
        public virtual EstadoSolicitudViewModel? SolIdEstadoSolicitudNavigation { get; set; } = null!;

        [ForeignKey("SolIdTipoSolicitud")]
        [InverseProperty("MceTbSolicitudTickets")]
        public virtual TipoSolicitudViewModel? SolIdTipoSolicitudNavigation { get; set; } = null!;

        [ForeignKey("SolIdUsuario")]
        [InverseProperty("MceTbSolicitudTickets")]
        public virtual UsuarioViewModel? SolIdUsuarioNavigation { get; set; } = null!;
    }
}
