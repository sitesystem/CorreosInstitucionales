using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaTools
{
    public static class CURP
    {
        readonly static string[] nombres_remover = ["MARIA", "MARÍA", "MA ", "MA.", "M.", "JOSE", "JOSÉ", "J ", "J."];
        readonly static char[] vocales = ['A', 'E', 'I', 'O', 'U'];
        readonly static Regex letras_remover = new Regex("[^A-Z ]");

        public static char CRC(string curp)
        {
            int sum = 0;
            int tmp;
            int value;
            int p = 18;

            foreach (char c in curp)
            {
                value = c >= 48 && c <= 57 ? c - 48 : c - 55;//( c - 65  +10 [MIN VAL])

                if (c >= 'O')// Ñ SE TOMA EN CUENTA
                {
                    value++;
                }

                tmp = p * value;

                sum += tmp;
                p--;
            }

            sum = sum % 10;
            sum = sum == 0 ? 0 : 10 - sum;

            return (char)(48 + sum);
        }//CRC

        public static string Generar(string nombre, string apellido1, string? apellido2, DateOnly fecha_nac, char sexo = 'H', string estado = "DF")
        {
            string result = string.Empty;

            //  NORMALIZAR NOMBRES
            string nombre_n = nombre.ToUpper();

            foreach (string nombre_a_quitar in nombres_remover)
            {
                if (nombre_n.StartsWith(nombre_a_quitar))
                {
                    nombre_n = nombre_n.Substring(nombre_a_quitar.Length).Trim();
                    break;
                }
            }

            // REMOVER CARACTERES NO PERMITIDOS (Ñ O NÚMEROS?)
            nombre_n = letras_remover.Replace(nombre_n, "X");

            char[] c_nombre = nombre_n.Cast<char>().ToArray();

            char[] c_apellido1 = letras_remover.Replace(apellido1.ToUpper(), "X").Cast<char>().ToArray();
            char[] c_apellido2 = letras_remover.Replace((apellido2??"X").ToUpper(), "X").Cast<char>().ToArray();

            Console.WriteLine($"{string.Join("", c_nombre)} {string.Join("", c_apellido1)} {string.Join("",c_apellido2)}");

            // INICIAL DEL PRIMER APELLIDO
            result += c_apellido1[0];
            c_apellido1 = c_apellido1.Skip(1).ToArray();

            // PRIMER VOCAL INTERNAL DEL PRIMER APELLIDO
            result += c_apellido1.First(c => vocales.Contains(c));

            // INICIAL DEL SEGUNDO APELLIDO
            result += c_apellido2[0];
            c_apellido2 = c_apellido2.Skip(1).ToArray();

            // INICAL DEL NOMBRE
            result += c_nombre[0];
            c_nombre = c_nombre.Skip(1).ToArray();

            // FECHA NACIMIENTO
            result += fecha_nac.ToString("yyMMdd");

            // SEXO
            result += sexo;

            // ESTADO
            result += estado;

            // PRIMER CONSONANTE DEL PRIMER APELLIDO
            result += c_apellido1.First(c => !vocales.Contains(c));

            // PRIMER CONSONANTE DEL PRIMER APELLIDO
            result += c_apellido2.First(c => !vocales.Contains(c));

            // PRIMER CONSONANTE DEL PRIMER APELLIDO
            result += c_nombre.First(c => !vocales.Contains(c));

            // EVITAR DUPLICADOS
            result += fecha_nac.Year >= 2000 ? 'A' : '0';

            // DIGITO VERIFICADOR
            result += CRC(result);

            return result;
        }//GENERAR

    }
}
