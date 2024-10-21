using System.Runtime.Serialization;

namespace Isban.MapsMB.Common.Entity.Response
{
    [DataContract]
    public class AdhesionesMEPResp
    {
        [DataMember]
        public long IdAdhesion { get; set; }

        [DataMember]
        public string Nup { get; set; }

        [DataMember]
        public string Segmento { get; set; }

        [DataMember]
        public string Canal { get; set; }

        [DataMember]
        public string SubCanal { get; set; }

        [DataMember]
        public long CuentaTitulos { get; set; }

        [DataMember]
        public long SucCtaOper { get; set; }

        [DataMember]
        public long NroCtaOper { get; set; }

        [DataMember]
        public long TipoCtaOper { get; set; }
                
        [DataMember]
        public decimal? ImporteEnviado { get; set; }

        [DataMember]
        public string CodEspecie { get; set; }

        [DataMember]
        public string Estado { get; set; }

        [DataMember]
        public string NumeroDocumento { get; set; }

        [DataMember]
        public string FechaNacimiento { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string Apellido { get; set; }


    }
}
