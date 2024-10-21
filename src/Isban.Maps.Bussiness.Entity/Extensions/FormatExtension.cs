using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Extensions
{
    public static class FormatExtension
    {
        public static string Ceros<T>(this T item, int cantidad)
        {
            return item.ToString().Trim().PadLeft(cantidad, '0');
        }
    }
}
