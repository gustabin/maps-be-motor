using Isban.Common.Data;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.DataAccess.DBResponse
{
    internal class ListaDeFondosDbResp : BaseResponse
    {
        [DBFieldDefinition(Name = "CUENTA_TITULO", ValueConverter = typeof(ResponseConvert<decimal>))]
        public decimal CuentaTitulo { get; set; }

        [DBFieldDefinition(Name = "CODIGO_FONDO", ValueConverter = typeof(ResponseConvert<string>))]
        public string CodigoFondo { get; set; }

        [DBFieldDefinition(Name = "DESCRIPCION", ValueConverter = typeof(ResponseConvert<string>))]
        public string DescripcionFondo { get; set; }

        [DBFieldDefinition(Name = "MONEDA", ValueConverter = typeof(ResponseConvert<string>))]
        public string MonedaFondo { get; set; }

        [DBFieldDefinition(Name = "AGRUPACION", ValueConverter = typeof(ResponseConvert<string>))]
        public string AgrupacionFondo { get; set; }

        [DBFieldDefinition(Name = "ORDEN", ValueConverter = typeof(ResponseConvert<string>))]
        public string OrdenAgrupacionFondo { get; set; }

        [DBFieldDefinition(Name = "FECHA_OP", ValueConverter = typeof(ResponseConvert<DateTime>))]
        public DateTime FechaOperacion { get; set; }

        [DBFieldDefinition(Name = "CANTIDAD_CUOTAPARTE", ValueConverter = typeof(ResponseConvert<decimal>))]
        public decimal CantidadCuotaparte { get; set; }

        [DBFieldDefinition(Name = "VALOR_CUOTAPARTE", ValueConverter = typeof(ResponseConvert<decimal>))]
        public decimal ValorCuotaparte { get; set; }

        [DBFieldDefinition(Name = "IMPORTE_OP", ValueConverter = typeof(ResponseConvert<decimal>))]
        public decimal ImporteOperacion { get; set; }

        [DBFieldDefinition(Name = "OPERACION", ValueConverter = typeof(ResponseConvert<string>))]
        public string TipoOperacion { get; set; }

        [DBFieldDefinition(Name = "COMPROBANTE", ValueConverter = typeof(ResponseConvert<string>))]
        public string Comprobante { get; set; }

    }
}
