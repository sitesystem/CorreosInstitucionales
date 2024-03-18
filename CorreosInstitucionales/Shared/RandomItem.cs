using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared
{
    public class RandomItem<T> where T : IConvertible
    {
        public int Chance { get; set; }
        public T Value { get; set; }

        public RandomItem(int chance, T item)
        {
            Chance = chance;
            Value = item;
        }

        public RandomItem(int chance, string value) 
        {
            Chance = chance;
            Value = (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
