

namespace Isban.MapsMB.Entity.Request
{
    using Common.Entity;
    using System.Runtime.Serialization;

    public class SimularAltaFondos : EntityBase
    {

        [DataMember]
        public string Tipo { get; set; }
        [DataMember]
        public string Cuenta { get; set; }
        [DataMember]
        public string Especie { get; set; }
        [DataMember]
        public string Moneda { get; set; }
        [DataMember]
        public decimal? Cuotapartes { get; set; }
        [DataMember]
        public decimal? Capital { get; set; }
        [DataMember]
        public string EspecieDestino { get; set; }
        [DataMember]
        public string UsuarioAsesor { get; set; }
        [DataMember]
        public string UsuarioRacf { get; set; }
        [DataMember]
        public string PasswordRacf { get; set; }
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
