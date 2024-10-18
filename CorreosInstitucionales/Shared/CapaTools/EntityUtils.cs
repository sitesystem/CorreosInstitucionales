using CorreosInstitucionales.Shared.CapaDataAccess.DBContext;
using CorreosInstitucionales.Shared.CapaEntities.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaTools
{
    public static class EntityUtils
    {
        /*******************************  CONVERSIÓN  *******************************/
        public static T FromNavigation<T,V>(V v)
            where T : new()
        {
            T result = new();

            if(v != null)
            {
                Type t = result.GetType();

                foreach (var prop in v.GetType().GetProperties())
                {
                    t.GetProperty(prop.Name)!.SetValue(result, prop.GetValue(v, null), null);
                }
            }

            return result;
        }
    }
}
