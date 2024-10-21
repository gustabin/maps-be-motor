using Isban.Mercados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Extensions
{
    public static class CommonExtension
    {
        public static T ParseGeneric<T>(this object variable)
        {
            T result = default(T);

            if (variable is T)
            {
                result = (T)variable;
            }
            else
            {
                try
                {
                    if (variable == null)
                    {
                        result = default(T);
                    }
                    else if (variable != null && Nullable.GetUnderlyingType(typeof(T)) != null)
                    {
                        //TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
                        var typeCon = Nullable.GetUnderlyingType(typeof(T));
                        result = (T)Convert.ChangeType(variable, typeCon);
                    }
                    else
                    {
                        result = (T)Convert.ChangeType(variable, typeof(T));
                    }
                }
                catch (Exception)
                {
                    throw new BusinessCodeException(1, string.Format("No se puede convertir el valor {0} del tipo {1} a {2}", variable, variable.GetType(), typeof(T)));
                }
            }

            return result;
        }
    }
}
