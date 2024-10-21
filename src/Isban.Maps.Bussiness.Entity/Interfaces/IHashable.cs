using System.Collections.Generic;

namespace Isban.Maps.Entity.Interfaces
{
    interface IDataInput<T>
    {
        void ValidarHash();
        void GenerarHash();

        List<T> GetValor();
    }
}
