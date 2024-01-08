using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.Request
{
    public class RequestViewModel_NoExtension
    {
        [Key]
        public int IdExtension { get; set; }

        [Column("extNoExtension")]
        [StringLength(10)]
        public string ExtNoExtension { get; set; } = null!;

        [Column("extIdAreaDepto")]
        public int ExtIdAreaDepto { get; set; }

        [Required]
        [Column("extStatus")]
        public bool ExtStatus { get; set; }

        [ForeignKey("ExtIdAreaDepto")]
        [InverseProperty("MceCatExtensiones")]
        public virtual RequestViewModel_AreaDepto? ExtIdAreaDeptoNavigation { get; set; }
    }
}
