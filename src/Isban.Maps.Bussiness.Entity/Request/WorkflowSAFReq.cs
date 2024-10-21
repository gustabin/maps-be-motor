

namespace Isban.MapsMB.Entity.Request
{
    using Common.Entity;
    using System.Runtime.Serialization;

    [DataContract]
    public class WorkflowSAFReq : EntityBase
    {
        [DataMember]
        public CabeceraConsulta Cabecera { get; set; }

        [DataMember]
        public string IdServicio { get; set; }
    }

    [DataContract]
    public class WorkflowCTRReq : EntityBase
    {
        [DataMember]
        public CabeceraConsulta Cabecera { get; set; }


        [DataMember]
        public string IdServicio { get; set; }

        [DataMember]
        public string Opcion { get; set; }

        [DataMember]
        public string OrdenCliente { get; set; }

        [DataMember]
        public string DatosCliente { get; set; }

        [DataMember]
        public string NumeroCuenta { get; set; }

        [DataMember]
        public string MotBaja { get; set; }

        [DataMember]
        public string FechaBaja { get; set; }

        [DataMember]
        public string CargaMensajes { get; set; }

    }


    public class AsociarCtaOperativaReq
    {
        [DataMember]
        public string NumeroCuenta { get; set; }

        [DataMember]
        public string UsuarioVerificacion { get; set; }

        [DataMember]
        public string CuentaCorriente { get; set; }

        [DataMember]
        public string CajaAhorro { get; set; }

        [DataMember]
        public string CuentaDolares { get; set; }

        [DataMember]
        public string Sucursal { get; set; }

    }
}
