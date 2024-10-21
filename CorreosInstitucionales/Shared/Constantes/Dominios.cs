using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.Constantes
{
    public static class Dominios
    {
        public static readonly string[] Prueba =
        [
            "localhost",
            "example.com"
        ];

        public static bool EsCorreoDePrueba(string correo)
        {
            return Prueba.Contains(correo.Split('@').Last().ToLower());
        }
    }
}
