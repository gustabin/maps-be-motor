using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Response
{
    [DataContract]
    public class MovimientoEspecieFondoRTF
    {
        [DataMember]
        public string Fondo { get; set; }

        [DataMember]
        public string CodigoMoneda { get; set; }

        [DataMember]
        public string SimboloMoneda { get; set; }

        [DataMember]
        public List<ItemMovimientoEspecieFondoRTF> ListaMovimientos { get; set; }

        [DataMember]
        public ItemMovimientoEspecieFondoRTF Total { get; set; }

        [DataMember]
        public ItemMovimientoEspecieFondoRTF SaldoAnterior { get; set; }

        [DataMember]
        public bool TieneLegales { get; set; }

        [DataMember]
        public List<string> ListaLegales { get; set; }

    }
}
