namespace Isban.MapsMB.Entity.Response
{
    using Common.Entity;
    using Isban.MapsMB.Entity.Constantes.Estructuras;
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class Mensaje
    {
        [DataMember]
        public long IdMensajes { get; set; }

        [DataMember]
        public long IdMyATitulo { get; set; }

        [DataMember]
        public long IdMyAMensaje { get; set; }

        [DataMember]
        public string IdNup { get; set; }

        [DataMember]
        public long NumeroDocumento { get; set; }

        [DataMember]
        public string NombreCliente { get; set; }

        [DataMember]
        public string ApellidoCliente { get; set; }

        [DataMember]
        public long NumeroComprobante { get; set; }

        [DataMember]
        public string CodEstadoProceso { get; set; }

        [DataMember]
        public string DescripcionFondo { get; set; }

        [DataMember]
        public long CuentaTitulo { get; set; }

        [DataMember]
        public long CuentaOperativa { get; set; }

        [DataMember]
        public DateTime FechaBaja { get; set; }

        [DataMember]
        public string Canal { get; set; }

        [DataMember]
        public string Destination { get; set; }

        [DataMember]
        public string EstadoEnvio { get; set; }

        [DataMember]
        public long CantidadIntentos { get; set; }

        [DataMember]
        public DateTime FechaPedido { get; set; }

        [DataMember]
        public DateTime FechaEnvio { get; set; }

        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string Segmento { get; set; }

        [DataMember]
        public string SubCanal { get; set; }

        [DataMember]
        public string IdServicio { get; set; }

        [DataMember]
        public string Operacion { get; set; }
    }
}
