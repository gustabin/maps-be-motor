

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Isban.MapsMB.Common.Entity.Response
{
    [DataContract]
    public class LoadSaldosResponse 
    {

        public List<Saldos> ListaSaldos { get; set; }
    }

    [DataContract]
    public class Saldos
    {
        [DataMember]
        public decimal? Cuenta { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public decimal? Sucursal { get; set; }

        [DataMember]
        public decimal? Asesor { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public decimal CAhorroPesos { get; set; }

        [DataMember]
        public decimal CAhorroDolares { get; set; }
    }
}
