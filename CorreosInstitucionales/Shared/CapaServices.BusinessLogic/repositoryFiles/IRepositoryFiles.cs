using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.repositoryFiles
{
    public interface IRepositoryFiles
    {
        public string GetPathByFolderIdFileName(string folder, int id, string fileName);
    }
}
