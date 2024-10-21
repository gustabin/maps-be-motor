using Isban.Common.Data;
using Isban.Mercados.DataAccess.OracleClient;
using Isban.Mercados.DataAccess.ConverterDBType;
using System;

namespace Isban.MapsMB.DataAccess.DBResponse
{
    internal class CargaSolicitudOrdenDbResp : BaseResponse
    {
        [DBFieldDefinition(Name = "ID_SOLICITUD_ORDENES", ValueConverter = typeof(ResponseConvert<long>))]
        public long IdSolicitudOrdenes { get; set; }

        [DBFieldDefinition(Name = "ID_ADHESIONES", ValueConverter = typeof(ResponseConvert<long?>))]
        public long? IdAdhesion { get; set; }

        [DBFieldDefinition(Name = "COD_ALTA_ADHESION", ValueConverter = typeof(ResponseConvert<long>))]
        public long CodAltaAdhesion { get; set; }

        [DBFieldDefinition(Name = "ID_NUP", ValueConverter = typeof(ResponseConvert<string>))]
        public string Nup { get; set; }

        [DBFieldDefinition(Name = "ID_SEGMENTO", ValueConverter = typeof(ResponseConvert<string>))]
        public string Segmento { get; set; }

        [DBFieldDefinition(Name = "ID_SERVICIO", ValueConverter = typeof(ResponseConvert<string>))]
        public string IdServicio { get; set; }

        [DBFieldDefinition(Name = "CANAL", ValueConverter = typeof(ResponseConvert<string>))]
        public string Canal { get; set; }

        [DBFieldDefinition(Name = "SUBCANAL", ValueConverter = typeof(ResponseConvert<string>))]
        public string SubCanal { get; set; }

        [DBFieldDefinition(Name = "CUENTA_TITULOS", ValueConverter = typeof(ResponseConvert<long>))]
        public long CuentaTitulos { get; set; }

        [DBFieldDefinition(Name = "SUC_CTA_OPER", ValueConverter = typeof(ResponseConvert<long>))]
        public long SucuCtaOper { get; set; }

        [DBFieldDefinition(Name = "NRO_CTA_OPER", ValueConverter = typeof(ResponseConvert<long>))]
        public long NroCtaOper { get; set; }

        [DBFieldDefinition(Name = "TIPO_CTA_OPER", ValueConverter = typeof(ResponseConvert<long>))]
        public long TipoCtaOper { get; set; }

        [DBFieldDefinition(Name = "SALDO_CUENTA_ANTES", ValueConverter = typeof(ResponseConvert<decimal?>))]
        public decimal? SaldoCuentaAntes { get; set; }

        [DBFieldDefinition(Name = "COD_MONEDA", ValueConverter = typeof(ResponseConvert<string>))]
        public string CodMoneda { get; set; }

        [DBFieldDefinition(Name = "FECHA_PROCESO", ValueConverter = typeof(ResponseConvert<System.DateTime>))]
        public DateTime FechaProceso { get; set; }

        [DBFieldDefinition(Name = "IN_REPROCESO", ValueConverter = typeof(ResponseConvert<long>))]
        public long InReproceso { get; set; }

        [DBFieldDefinition(Name = "COD_ESTADO_PROCESO", ValueConverter = typeof(ResponseConvert<string>))]
        public string CodEstadoProceso { get; set; }

        [DBFieldDefinition(Name = "OBSERVACION", ValueConverter = typeof(ResponseConvert<string>))]
        public string Observacion { get; set; }

        [DBFieldDefinition(Name = "DE_DISCLAIMER", ValueConverter = typeof(ResponseConvert<string>))]
        public string DescDisclaimer { get; set; }

        [DBFieldDefinition(Name = "FE_VIGENCIA_DESDE", ValueConverter = typeof(ResponseConvert<System.DateTime>))]
        public DateTime FecVigenciaDesde { get; set; }

        [DBFieldDefinition(Name = "FE_VIGENCIA_HASTA", ValueConverter = typeof(ResponseConvert<System.DateTime>))]
        public DateTime FecVigenciaHasta { get; set; }

        [DBFieldDefinition(Name = "SALDO_MIN_OPERACION", ValueConverter = typeof(ResponseConvert<decimal?>))]
        public decimal? SaldoMinOperacion { get; set; }

        [DBFieldDefinition(Name = "SALDO_MAX_OPERACION", ValueConverter = typeof(ResponseConvert<decimal?>))]
        public decimal? SaldoMaxOperacion { get; set; }

        [DBFieldDefinition(Name = "SALDO_ENVIADO", ValueConverter = typeof(ResponseConvert<decimal?>))]
        public decimal? SaldoEnviado { get; set; }

        [DBFieldDefinition(Name = "COD_FONDO", ValueConverter = typeof(ResponseConvert<string>))]
        public string CodigoFondo { get; set; }

        [DBFieldDefinition(Name = "COD_ESPECIE", ValueConverter = typeof(ResponseConvert<string>))]
        public string CodEspecie { get; set; }

        [DBFieldDefinition(Name = "SALDO_MIN_POR_FONDO", ValueConverter = typeof(ResponseConvert<long>))]
        public long SaldoMinPorFondo { get; set; }

        [DBFieldDefinition(Name = "SALDO_RESCATE_POR_FONDO", ValueConverter = typeof(ResponseConvert<decimal?>))]
        public decimal? SaldoRescatePorFondo { get; set; }

        [DBFieldDefinition(Name = "NU_ORDEN_ORIGEN", ValueConverter = typeof(ResponseConvert<long>))]
        public long NumOrdenOrigen { get; set; }

        [DBFieldDefinition(Name = "SALDO_MIN_DEJAR_CTA", ValueConverter = typeof(ResponseConvert<decimal>))]
        public decimal SaldoMinDejarCta { get; set; }

        [DBFieldDefinition(Name = "COD_PRODUCTO", ValueConverter = typeof(ResponseConvert<string>))]
        public string CodProducto { get; set; }

        [DBFieldDefinition(Name = "COD_CANAL", ValueConverter = typeof(ResponseConvert<string>))]
        public string CodCanal { get; set; }

        [DBFieldDefinition(Name = "PRODUCTO", ValueConverter = typeof(ResponseConvert<string>))]
        public string Producto { get; set; }

        [DBFieldDefinition(Name = "TIPO_OPERACION", ValueConverter = typeof(ResponseConvert<string>))]
        public string TipoOperacion { get; set; }

        [DBFieldDefinition(Name = "COMPRA_VENTA", ValueConverter = typeof(ResponseConvert<int>))]
        public int CompraVenta { get; set; }

        [DBFieldDefinition(Name = "FECHA_BAJA", ValueConverter = typeof(ResponseConvert<System.DateTime>))]
        public DateTime FechaBaja { get; set; }

        [DBFieldDefinition(Name = "FECHA_EJECUCION", ValueConverter = typeof(ResponseConvert<System.DateTime>))]
        public DateTime FechaDeEjecucion { get; set; }

        [DBFieldDefinition(Name = "OPERACION", ValueConverter = typeof(ResponseConvert<string>))]
        public string Operacion { get; set; }
    }
}
