using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.Constantes
{
    public static class TipoPersonal
    {
        public const int ALUMNO             = 1;
        public const int EGRESADO           = 2;
        public const int MAESTRIA           = 3;
        public const int ADMINISTRATIVO     = 4;
        public const int DOCENTE            = 5;
        public const int HONORARIOS         = 6;
        public const int PLATAFORMA_SACI    = 7;

        public static readonly Dictionary<int, string> Nombre = new()
        {
            {ALUMNO,            "ALUMNO" },//ALUMNO
            {EGRESADO,          "EGRESADO" },//EGRESADO
            {MAESTRIA,          "EGRESADO" },//ALUMNO MAESTRIA
            {ADMINISTRATIVO,    "ADMINISTRATIVO" },//PAAE (ADMINISTRATIVO)
            {DOCENTE,           "DOCENTE" },//DOCENTE
            {HONORARIOS,        "HONORARIOS" },//HONORARIOS
            {PLATAFORMA_SACI,   "ADMINISTRATIVO" },//PERSONAL UDI - <!> PREGUNTAR
        };

    }
}
