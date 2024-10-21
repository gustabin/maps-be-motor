using System.Runtime.Serialization;

namespace Isban.MapsMB.Common.Entity.Response
{
    [DataContract]
    public class SaldoCuentaResp
    {
        [DataMember]
        public string CodMoneda { get; set; }

        [DataMember]
        public long CuentaTitulos { get; set; }

        [DataMember]
        public long NumeroCuentaOperativa { get; set; }

        [DataMember]
        public string Nup { get; set; }

        [DataMember]
        public decimal? SaldoActual { get; set; }

        [DataMember]
        public string Segmento { get; set; }

        [DataMember]
        public long SucuCtaOper { get; set; }

        [DataMember]
        public long TipoCtaOper { get; set; }

        [DataMember]
        public string Observacion { get; set; }

        [DataMember]
        public long Id { get; set; }
    }
}
