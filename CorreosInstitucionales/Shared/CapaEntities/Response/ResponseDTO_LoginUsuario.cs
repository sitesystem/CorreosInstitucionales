using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using CorreosInstitucionales.Shared.CapaDataAccess.DBContext;

namespace CorreosInstitucionales.Shared.CapaEntities.Response
{
    public class ResponseDTO_LoginUsuario : MpTbUsuario
    {
        [Column("usuToken")]
        [StringLength(300)]
        public string UsuToken { get; set; } = null!;
    }
}
