using System;
using System.Runtime.Serialization;

namespace Isban.MapsMB.Common.Entity.Request
{
    public class InsertarDisclaimerEriMEPCompraReq
    {
        [DataMember]
        public long IdAdhesion { get; set; }

        [DataMember]
        public string ComprobanteCompra { get; set; }

        [DataMember]
        public decimal CantidadNominales { get; set; }

        [DataMember]
        public string Estado { get; set; }

        [DataMember]
        public string CantidadDisclaimers { get; set; }

        [DataMember]
        public long IdEvaluacion { get; set; }

        [DataMember]
        public string TextoDisclaimer { get; set; }

        [DataMember]
        public short TipoDisclaimer { get; set; }

        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string Ip { get; set; }

        [DataMember]
        public DateTime FechaConfirmacionCompra { get; set; }

        [DataMember]
        public string Nup { get; set; }

        [DataMember]
        public string NuDni { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string Apellido { get; set; }
    }
}
