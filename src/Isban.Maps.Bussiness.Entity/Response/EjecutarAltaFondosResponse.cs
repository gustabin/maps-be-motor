using System.Runtime.Serialization;

namespace Isban.MapsMB.Entity.Response
{
    [DataContract]
    public class EjecutarAltaFondosResponse
    {
        [DataMember]
        public string NumOrden { get; set; }
        [DataMember]
        public long? NumCertificadoFm { get; set; }
        [DataMember]
        public decimal? CuotapartesFm { get; set; }
        [DataMember]
        public decimal? CapitalFm { get; set; }
        [DataMember]
        public string DentroDelPerfil { get; set; }
        [DataMember]
        public string Disclaimer { get; set; }
        [DataMember]
        public decimal? Cotizacion { get; set; }
    }

}