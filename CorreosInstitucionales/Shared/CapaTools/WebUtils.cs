using CorreosInstitucionales.Shared.CapaServices.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.Utils
{
    public static class WebUtils
    {
        public static async Task<List<T>> ListAll<T>(IGenericService<T> service, bool filterByStatus = true)
        {
            var response = await service.GetAllDataAsync(filterByStatus);

            if (response is not null)
            {
                return response.Data ?? new List<T>();
            }

            return new List<T>();
        }
    }
}
