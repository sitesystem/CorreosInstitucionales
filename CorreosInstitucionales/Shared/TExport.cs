using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared
{
    public class TExport<T>
    {
        public T Value { get; set; }
        public bool Update { get; set; }
        public bool Notify { get; set; }

        public int Status { get; set; }

        public TExport(T value, int status, bool update = false, bool notify = false)
        {
            this.Value = value;
            this.Update = update;
            this.Notify = notify;
            this.Status = status;
        }
    }
}
