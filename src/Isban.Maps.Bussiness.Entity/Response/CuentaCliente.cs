namespace Isban.MapsMB.Entity.Response
{
    using Isban.MapsMB.Common.Entity;
    using System.Runtime.Serialization;
    using System;

    [DataContract]
    public class CuentaCliente : EntityBase
    {
        [DataMember]
        public string NroCta { get; set; }

        [DataMember]
        public long? TipoCta { get; set; }

        [DataMember]
        public string DescripcionTipoCta { get; set; }

        [DataMember]
        public string CodProducto { get; set; }

        [DataMember]
        public string CodSubproducto { get; set; }

        [DataMember]
        public string SucursalCta { get; set; }

        [DataMember]
        public string CodigoMoneda { get; set; }

        [DataMember]
        public string SegmentoCuenta { get; set; }

        [DataMember]
        public string CuentaBloqueada { get; set; }

        [DataMember]
        public string CodigoBloqueo { get; set; }

        [DataMember]
        public string MotivoBloqueo { get; set; }

        [DataMember]
        public string DetalleBloqueo { get; set; }

        [DataMember]
        public string CalidadParticipacion { get; set; }

        [DataMember]
        public long OrdenParticipacion { get; set; }

        public override bool Validar()
        {
            throw new NotImplementedException();
        }
    }
}