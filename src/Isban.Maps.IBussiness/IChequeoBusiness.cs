using Isban.MapsMB.Common.Entity;
using System.Collections.Generic;

namespace Isban.MapsMB.IBussiness
{
    public interface IChequeoBusiness
    {
        List<ChequeoAcceso> Chequeo(EntityBase entity);
    }
}
