﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MC_catSemestres")]
public partial class McCatSemestre
{
    [Key]
    public int IdSemestre { get; set; }

    [Column("semNombre")]
    [StringLength(100)]
    public string SemNombre { get; set; } = null!;
}