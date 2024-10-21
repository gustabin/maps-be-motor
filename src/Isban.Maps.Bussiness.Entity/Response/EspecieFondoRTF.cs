using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Response
{
    [DataContract]
    public class EspecieFondoRTF
    {
        [DataMember]
        public string Tipo { get; set; }

        [DataMember]
        public string Fondo { get; set; }

        [DataMember]
        public string CantidadCuotapartes { get; set; }

        [DataMember]
        public string ValorCuotaparte { get; set; }

        [DataMember]
        public decimal? TenenciaValuada { get; set; }

        [DataMember]
        public string CodigoMoneda { get; set; }

        [DataMember]
        public string SimboloMoneda { get; set; }

        public long CodigoFondo { get; set; }

        [DataMember]
        public bool TieneLegales { get; set; }

        [DataMember]
        public List<string> ListaLegales { get; set; }
    }
}
