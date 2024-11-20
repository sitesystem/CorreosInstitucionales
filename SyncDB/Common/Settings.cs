using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncDB.Common
{
    public class Settings
    {
        public Dictionary<string,string?> ConnectionStrings { get; set; } = new Dictionary<string,string?>();
    }
}
