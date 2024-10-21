using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Request
{
    using Isban.Maps.Entity.Base;
    using Isban.MapsMB.Common.Entity;
    using System.Runtime.Serialization;
    using System;

    [DataContract]
    public class GetDatosCliente : EntityBase
    {
        [DataMember]
        public string DatoConsulta { get; set; }

        [DataMember]
        public string TipoBusqueda { get; set; }

        [DataMember]
        public string CuentasRespuesta { get; set; }

        [DataMember]
        public string IdServicio { get; set; }

        [DataMember]
        public string Titulares { get; set; }

        public override bool Validar()
        {
            throw new NotImplementedException();
        }
    }
}
