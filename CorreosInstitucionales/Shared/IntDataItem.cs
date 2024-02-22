using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared
{
    public class IntDataItem
    {
        public int Value { get; set; }
        public string Text { get; set; }

        public IntDataItem()
        {
            Value = 0;
            Text = "UNDEFINED";
        }

        public IntDataItem (int v, string t)
        {
            this.Value = v;
            this.Text = t;
        }

    }
}
