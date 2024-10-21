using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Extensions
{
    public static class EntityExtension
    {
        public static bool Is<T>(this T item, params T[] list)
        {
            return list.Contains(item);
        }
    }
}
