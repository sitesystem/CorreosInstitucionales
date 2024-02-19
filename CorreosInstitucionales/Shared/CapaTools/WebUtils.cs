using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaServices.BusinessLogic;

namespace CorreosInstitucionales.Shared.Utils
{
    public static class WebUtils
    {
        public static async Task<List<T>> ListByStatusAsync<T>(IGenericService<T> service, bool filterByStatus = true)
        {
            List<T> result = new();

            try
            {
                var response = await service.GetAllDataByStatusAsync(filterByStatus);

                if(response is not null && response.Data is not null)
                    result = response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message} ");
            }

            return result;
        }
    }
}
