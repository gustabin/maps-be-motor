
namespace Isban.MapsMB.Entity.Request
{
    using Common.Entity;
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class SimulaPdcReq : EntityBase
    {
        [DataMember]
        public long IdSimCuentaPDC { get; set; }        

        [DataMember]
        public long CuentaTitulos { get; set; }

        [DataMember]
        public string NroCtaOperativa { get; set; }

        [DataMember]
        public string TipoCtaOperativa { get; set; }

        [DataMember]
        public string SucursalCtaOperativa { get; set; }

        [DataMember]
        public string CodigoMoneda { get; set; }

        [DataMember]
        public DateTime FechaAlta { get; set; }
           
        [DataMember]
        public string Subcanal { get; set; }
                  
        [DataMember]
        public string Operacion { get; set; }

        [DataMember]
        public string Producto { get; set; }

        [DataMember]
        public string Subproducto { get; set; }
           
        [DataMember]
        public DateTime SiguienteDiaHabil { get; set; }


        [DataMember]
        public string Motivo { get; set; }

        public override bool Validar()
        {
            throw new NotImplementedException();
        }
    }
}
