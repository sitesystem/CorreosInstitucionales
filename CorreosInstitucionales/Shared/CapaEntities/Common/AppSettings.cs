﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.Common
{
    public class AppSettings
    {
        public string? Secreto { get; set; }
        public string ClaveEscuela { get; set; } = string.Empty;
    }
}
