using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.repositoryFiles
{
    public class RRepositoryFiles : IRepositoryFiles
    {
        public string GetPathByFolderIdFileName(string folder, int id, string fileName)
        {
            return $"/Repositorio/{folder}/{id}/{fileName}";
        }
    }
}
