﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.Constantes
{
    public enum TipoEstadoSolicitud : int
    {
        ALTA = 1,
        PENDIENTE = 2,
        EN_PROCESO = 3,
        ATENDIDA = 4,
        ENCUESTA_CALIDAD = 5,
        CANCELADA = 6,
    }
}
