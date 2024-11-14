using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContextCentral;

[Table("tbUsuarios")]
public partial class TbUsuario
{
    /// <summary>
    /// PK ID Único del Usuario
    /// </summary>
    [Key]
    public int IdUsuario { get; set; }

    /// <summary>
    /// Nombre(s) del Usuari@
    /// </summary>
    [Column("usuNombres")]
    [StringLength(200)]
    [Unicode(false)]
    public string UsuNombres { get; set; } = null!;

    /// <summary>
    /// Primer Apellido del Usuari@
    /// </summary>
    [Column("usuPrimerApellido")]
    [StringLength(150)]
    [Unicode(false)]
    public string UsuPrimerApellido { get; set; } = null!;

    /// <summary>
    /// Segundo Apellido del Usuari@
    /// </summary>
    [Column("usuSegundoApellido")]
    [StringLength(150)]
    [Unicode(false)]
    public string? UsuSegundoApellido { get; set; }

    /// <summary>
    /// CURP del Usuari@
    /// </summary>
    [Column("usuCURP")]
    [StringLength(18)]
    [Unicode(false)]
    public string UsuCurp { get; set; } = null!;

    /// <summary>
    /// Foto / Imágen de Perfil en formato .jpg / .png
    /// </summary>
    [Column("usuFotoPerfil")]
    [StringLength(300)]
    [Unicode(false)]
    public string? UsuFotoPerfil { get; set; }

    /// <summary>
    /// Fecha de Nacimiento
    /// </summary>
    [Column("usuFechaNacimiento")]
    public DateOnly? UsuFechaNacimiento { get; set; }

    /// <summary>
    /// Edad del Usuari@
    /// </summary>
    [Column("usuEdad")]
    public byte? UsuEdad { get; set; }

    /// <summary>
    /// Género / Sexo del Usuari@
    /// </summary>
    [Column("usuGenero")]
    [StringLength(20)]
    [Unicode(false)]
    public string? UsuGenero { get; set; }

    /// <summary>
    /// Estado Civil del Usuari@
    /// </summary>
    [Column("usuEstadoCivil")]
    [StringLength(50)]
    [Unicode(false)]
    public string? UsuEstadoCivil { get; set; }

    /// <summary>
    /// Nacionalidad del Usuari@
    /// </summary>
    [Column("usuNacionalidad")]
    [StringLength(50)]
    [Unicode(false)]
    public string? UsuNacionalidad { get; set; }

    /// <summary>
    /// Número de Celular Anterior por cambio o pérdida
    /// </summary>
    [Column("usuNoCelularAnterior")]
    [StringLength(20)]
    [Unicode(false)]
    public string? UsuNoCelularAnterior { get; set; }

    /// <summary>
    /// Número de Celular Actual / Nuevo
    /// </summary>
    [Column("usuNoCelularActual")]
    [StringLength(20)]
    [Unicode(false)]
    public string? UsuNoCelularActual { get; set; }

    /// <summary>
    /// Correo Personal Anterior por cambio o pérdida
    /// </summary>
    [Column("usuCorreoPersonalAnterior")]
    [StringLength(100)]
    [Unicode(false)]
    public string? UsuCorreoPersonalAnterior { get; set; }

    /// <summary>
    /// Correo Personal Actual / Nuevo
    /// </summary>
    [Column("usuCorreoPersonalActual")]
    [StringLength(100)]
    [Unicode(false)]
    public string? UsuCorreoPersonalActual { get; set; }

    /// <summary>
    /// Contraseña con la que se ingresará a las Plataformas / Aplicaciones
    /// </summary>
    [Column("usuContraseniaPlataformas")]
    [StringLength(300)]
    [Unicode(false)]
    public string UsuContraseniaPlataformas { get; set; } = null!;

    /// <summary>
    /// 0 = Ingresa a la Plataforma, 1 = Pide ingresar una nueva contraseña y sale de la Aplicación
    /// </summary>
    [Column("usuRecuperacionContrasenia")]
    public bool UsuRecuperacionContrasenia { get; set; }

    [Column("usuCorreoInstitucional")]
    [StringLength(100)]
    [Unicode(false)]
    public string? UsuCorreoInstitucional { get; set; }

    [Column("usuContraseniaCorreoInstitucional")]
    [StringLength(100)]
    [Unicode(false)]
    public string? UsuContraseniaCorreoInstitucional { get; set; }

    /// <summary>
    /// Fecha y Hora en que se dió de alta al Usuari@
    /// </summary>
    [Column("usuFechaHoraAlta", TypeName = "datetime")]
    public DateTime? UsuFechaHoraAlta { get; set; }

    [Column("usuFechaHoraActualizacion", TypeName = "datetime")]
    public DateTime UsuFechaHoraActualizacion { get; set; }

    [Column("usuIdDireccion")]
    public int? UsuIdDireccion { get; set; }

    [Column("usuDetallesDireccion")]
    [StringLength(250)]
    [Unicode(false)]
    public string? UsuDetallesDireccion { get; set; }

    /// <summary>
    /// Estado { 1 = Activo, 0 = Inactivo }
    /// </summary>
    [Column("usuStatus")]
    public bool? UsuStatus { get; set; }

    [InverseProperty("AluIdUsuarioNavigation")]
    public virtual ICollection<TbAlumno> TbAlumnos { get; set; } = new List<TbAlumno>();

    [InverseProperty("EmpIdUsuarioNavigation")]
    public virtual ICollection<TbEmpleado> TbEmpleados { get; set; } = new List<TbEmpleado>();

    [ForeignKey("UsuIdDireccion")]
    [InverseProperty("TbUsuarios")]
    public virtual TbDireccione? UsuIdDireccionNavigation { get; set; }
}
