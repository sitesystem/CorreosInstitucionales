using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.Constantes
{
    public static class Catalogo
    {
        public static TDataItem<string>[] Semestres = 
        [
            new ("1","Primer"),
            new ("2","Segundo"),
            new ("3","Tercer"),
            new ("4","Cuarto"),
            new ("5","Quinto"),
            new ("6","Sexto"),
            new ("7","Séptimo"),
            new ("8","Octavo"),
        ];
    }
}
