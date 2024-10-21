

namespace Isban.Maps.IBussiness
{
    using Entity;
    using Entity.Base;
    using System.Collections.Generic;
    public interface IChequeoBusiness
    {
        List<ChequeoAcceso> Chequeo(EntityBase entity);
    }

}
