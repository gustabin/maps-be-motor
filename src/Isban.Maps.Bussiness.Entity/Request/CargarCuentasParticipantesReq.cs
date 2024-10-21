using System.Runtime.Serialization;

namespace Isban.MapsMB.Common.Entity.Request
{
    public class CargarCuentasParticipantesReq : EntityBase
    {
        [DataMember]
        public long? CuentaTitulo { get; set; }
        [DataMember]
        public long? CtaOperativa { get; set; }
        [DataMember]
        public long? SucCtaOperativa { get; set; }
        [DataMember]
        public long? TipoCtaOperativa { get; set; }
        [DataMember]
        public string ProductoOperativa { get; set; }
        [DataMember]
        public string SubproductoOperativa { get; set; }
        [DataMember]
        public string SubproductoTitular { get; set; }
        [DataMember]
        public string ProductoTit { get; set; }
        [DataMember]
        public string Procesado { get; set; }

        [DataMember]
        public string RelacionCtasOperativas { get; set; }
        [DataMember]
        public int? CantParticipantes { get; set; }
        [DataMember]
        public long CodAltAdAhesion { get; set; }
    }

    public class ActualizarAdhesionRepatriacionReq : EntityBase
    {
        [DataMember]
        public long? CuentaTitulo { get; set; }
    
        [DataMember]
        public long CodAltAdAhesion { get; set; }
    }
}
