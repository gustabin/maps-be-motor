using Isban.PDC.Middleware.DataAccess.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.DataAccess.IATXRequest
{
    public class AltaCuentaRequest : BaseIaxtRequest
    {
        public string Cónyuge_Indicador_cta_mancomunado { get; set; }
        public string Adhiere_paquete { get; set; }
        public string Tipo_intervención { get; set; }
        public string Sucursal { get; set; }
        public string Producto { get; set; }
        public string SubProducto { get; set; }
        public string Nro_Cuenta { get; set; }
        public string Firmante { get; set; }
        public string Código_moneda { get; set; }
        public string Sucursal_operación { get; set; }
        public string Canal_venta { get; set; }
        public string Tipo_uso { get; set; }

        public string Cod_condicion { get; set; }


        public string Forma_Operar { get; set; }

        public string ADHIERE_RIOLINE_RIOSELF { get; set; }
        public string DOMICILIO_CORRESPONDENCIA { get; set; }
        public string SEC_DOM_LABORAL { get; set; }
        public string CuentaCorriente { get; set; }
        public string CajaAhorro { get; set; }
    }

    public class GeneracionCuentaRequest : BaseIaxtRequest
    {

        public string Sucursal { get; set; }


        public string Producto { get; set; }
        public string Subproducto { get; set; }
    }

    [DataContract]
    public class ConsultaCuentaRequest : BaseIaxtRequest
    {
        [DataMember]
        public string UsuarioV { get; set; }

        [DataMember]
        public string Nro_Cuenta { get; set; }
        [DataMember]
        public string CuentaC { get; set; }
        [DataMember]
        public string CajaAhorro { get; set; }
        [DataMember]
        public string CuentaDolares { get; set; }
        [DataMember]
        public string Sucursal { get; set; }

    }
    public class CmbreclicRequest : BaseIaxtRequest
    {
        public string Oficina_Contrato { get; set; }
        public string Nro_Cuenta { get; set; }
        public string Aplicacion { get; set; }
        public string MotBaja { get; set; }
        public string FechaBaja { get; set; }
        public string Opcion { get; set; }


        public string DatosCliente { get; set; }
        public string OrdenCliente { get; set; }
        public string Responsabilidad { get; set; }
        public string CodigoCondicion { get; set; }
        public string Categoria { get; set; }
        public string FormaOperar { get; set; }
        public string GrupoEmpresa { get; set; }
        public string ADHIERE_RIOLINE_RIOSELF { get; set; }
        public string TitularSeg { get; set; }



        public string DatosCliente1 { get; set; }
        public string OrdenCliente1 { get; set; }
        public string Responsabilidad1 { get; set; }
        public string CodigoCondicion1 { get; set; }
        public string Categoria1 { get; set; }
        public string FormaOperar1 { get; set; }
        public string GrupoEmpresa1 { get; set; }
        public string ADHIERE_RIOLINE_RIOSELF1 { get; set; }
        public string TitularSeg1 { get; set; }



        public string DatosCliente2 { get; set; }
        public string OrdenCliente2 { get; set; }
        public string Responsabilidad2 { get; set; }
        public string CodigoCondicion2 { get; set; }
        public string Categoria2 { get; set; }
        public string FormaOperar2 { get; set; }
        public string GrupoEmpresa2 { get; set; }
        public string ADHIERE_RIOLINE_RIOSELF2 { get; set; }
        public string TitularSeg2 { get; set; }



        public string DatosCliente3 { get; set; }
        public string OrdenCliente3 { get; set; }
        public string Responsabilidad3 { get; set; }
        public string CodigoCondicion3 { get; set; }
        public string Categoria3 { get; set; }
        public string FormaOperar3 { get; set; }
        public string GrupoEmpresa3 { get; set; }
        public string ADHIERE_RIOLINE_RIOSELF3 { get; set; }
        public string TitularSeg3 { get; set; }


        public string DatosCliente4 { get; set; }
        public string OrdenCliente4 { get; set; }
        public string Responsabilidad4 { get; set; }
        public string CodigoCondicion4 { get; set; }
        public string Categoria4 { get; set; }
        public string FormaOperar4 { get; set; }
        public string GrupoEmpresa4 { get; set; }
        public string ADHIERE_RIOLINE_RIOSELF4 { get; set; }
        public string TitularSeg4 { get; set; }


        public string DatosCliente5 { get; set; }
        public string OrdenCliente5 { get; set; }
        public string Responsabilidad5 { get; set; }
        public string CodigoCondicion5 { get; set; }
        public string Categoria5 { get; set; }
        public string FormaOperar5 { get; set; }
        public string GrupoEmpresa5 { get; set; }
        public string ADHIERE_RIOLINE_RIOSELF5 { get; set; }
        public string TitularSeg5 { get; set; }



        public string DatosCliente6 { get; set; }
        public string OrdenCliente6 { get; set; }
        public string Responsabilidad6 { get; set; }
        public string CodigoCondicion6 { get; set; }
        public string Categoria6 { get; set; }
        public string FormaOperar6 { get; set; }
        public string GrupoEmpresa6 { get; set; }
        public string ADHIERE_RIOLINE_RIOSELF6 { get; set; }
        public string TitularSeg6 { get; set; }



        public string DatosCliente7 { get; set; }
        public string OrdenCliente7 { get; set; }
        public string Responsabilidad7 { get; set; }
        public string CodigoCondicion7 { get; set; }
        public string Categoria7 { get; set; }
        public string FormaOperar7 { get; set; }
        public string GrupoEmpresa7 { get; set; }
        public string ADHIERE_RIOLINE_RIOSELF7 { get; set; }
        public string TitularSeg7 { get; set; }


    }
}
