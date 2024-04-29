using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared
{
    public class TDataItem<T>
    {
        public T Value { get; set; }
        public string Text { get; set; }

        public TDataItem(T v, string t)
        {
            this.Value = v;
            this.Text = t;
        }

        public override string ToString()
        {
            return $"{Text} = {Value}";
        }
    }
}
