using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Request
{
    [DataContract]
    public class GetTitulares : EntityBase
    {
        [DataMember]
        public string CuentaTitulos { get; set; }

        [DataMember]
        public string NroCtaOperativa { get; set; }

        [DataMember]
        public string SucursalCtaOperativa { get; set; }

        [DataMember]
        public string TipoCtaOperativa { get; set; }

        [DataMember]
        public string CodMoneda { get; set; }

        [DataMember]
        public string Canal { get; set; }

        [DataMember]
        public string SubCanal { get; set; }

        [DataMember]
        public bool TieneErrores { get; set; }

        [DataMember]
        public string Errores { get; set; }

        //[DataMember]
        //public Cabecera Cabecera { get; set; }
    }
}
