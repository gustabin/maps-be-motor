using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity
{
    public class AltaAdhesionMAPSReq : EntityBase
    {  
        [DataMember]
        public string ID_NUP { get; set; }
        [DataMember]
        public string COD_FONDO { get; set; }
        [DataMember]
        public string CUENTA_TITULOS { get; set; }
        [DataMember]
        public decimal IMPORTE_BRUTO { get; set; }
        [DataMember]
        public string TIPO_CTA_OPER { get; set; }
        [DataMember]
        public string SUC_CTA_OPER { get; set; }
        [DataMember]
        public string NRO_CTA_OPER { get; set; }
        [DataMember]
        public string COD_MONEDA { get; set; }

        public override bool Validar()
        {
            throw new NotImplementedException();
        }
    }
}
