

using System.Runtime.Serialization;

namespace Isban.MapsMB.Common.Entity.Response
{
    [DataContract]
    public class SaldoConcertadoNoLiquidadoResponse
    {
        [DataMember]
        public decimal? Saldo { get; set; }
    }
}
