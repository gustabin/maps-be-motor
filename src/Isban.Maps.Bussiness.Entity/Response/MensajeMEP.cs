namespace Isban.MapsMB.Entity.Response
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class MensajeMEP
    {
        [DataMember]
        public long IdAhesion { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string Apellido { get; set; }

        [DataMember]
        public string Dni { get; set; }

        [DataMember]
        public DateTime FechaConfirmacionCompra { get; set; }

        [DataMember]
        public string ComprobanteCompra { get; set; }

        [DataMember]
        public string Segmento { get; set; }

        [DataMember]
        public string Nup { get; set; }

        [DataMember]
        public string Canal { get; set; }

        [DataMember]
        public long CuentaOperativa { get; set; }

        [DataMember]
        public long CuentaTitulo { get; set; }
    }
}
