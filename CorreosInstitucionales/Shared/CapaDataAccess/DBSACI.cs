using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorreosInstitucionales.Shared.CapaDataAccess.DBContext;

namespace CorreosInstitucionales.Shared.CapaDataAccess
{
    public class DBSACI : DbCorreosInstitucionalesUpiicsaContext
    {
        protected readonly string _connectionString = string.Empty;

        public DBSACI(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString,
            ob => ob.UseCompatibilityLevel(120));
        }
    }
}
