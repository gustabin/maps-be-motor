using System;
using System.Runtime.Serialization;

namespace Isban.MapsMB.Common.Entity.Response
{
    [DataContract]
    public class ListaDeFondosResponse
    {
        [DataMember]
        public decimal CuentaTitulo { get; set; }

        [DataMember]
        public string CodigoFondo { get; set; }

        [DataMember]
        public string DescripcionFondo { get; set; }

        [DataMember]
        public string MonedaFondo { get; set; }

        [DataMember]
        public string AgrupacionFondo { get; set; }

        [DataMember]
        public string OrdenAgrupacionFondo { get; set; }

        [DataMember]
        public DateTime FechaOperacion { get; set; }

        [DataMember]
        public decimal CantidadCuotaparte { get; set; }

        [DataMember]
        public decimal ValorCuotaparte { get; set; }

        [DataMember]
        public decimal ImporteOperacion { get; set; }
    }
}
