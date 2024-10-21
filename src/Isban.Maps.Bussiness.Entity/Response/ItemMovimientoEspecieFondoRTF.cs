using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Response
{
    [DataContract]
    public class ItemMovimientoEspecieFondoRTF
    {
        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public string Concepto { get; set; }

        [DataMember]
        public string Comprobante { get; set; }

        [DataMember]
        public string CantidadCuotapartes { get; set; }

        [DataMember]
        public string ValorCuotaparte { get; set; }

        [DataMember]
        public decimal? ImporteNeto { get; set; }
    }
}
