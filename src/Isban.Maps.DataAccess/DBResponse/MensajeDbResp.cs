using Isban.Common.Data;
using Isban.Mercados.DataAccess.OracleClient;
using Isban.Mercados.DataAccess.ConverterDBType;
using System;

namespace Isban.MapsMB.DataAccess.DBResponse
{
    internal class MensajeDbResp : BaseResponse
    {           
        [DBFieldDefinition(Name = "ID_MENSAJES", ValueConverter = typeof(ResponseConvert<long>))]
        public long IdMensajes { get; set; }

        [DBFieldDefinition(Name = "ID_MYA_TITULO", ValueConverter = typeof(ResponseConvert<long>))]
        public long IdMyATitulo { get; set; }

        [DBFieldDefinition(Name = "ID_MYA_MENSAJE", ValueConverter = typeof(ResponseConvert<long>))]
        public long IdMyAMensaje { get; set; }

        [DBFieldDefinition(Name = "ID_NUP", ValueConverter = typeof(ResponseConvert<string>))]
        public string IdNup { get; set; }

        [DBFieldDefinition(Name = "NU_DNI", ValueConverter = typeof(ResponseConvert<long>))]
        public long NumeroDocumento { get; set; }

        [DBFieldDefinition(Name = "NOM_CLIENTE", ValueConverter = typeof(ResponseConvert<string>))]
        public string NombreCliente { get; set; }

        [DBFieldDefinition(Name = "APELLIDO_CLIENTE", ValueConverter = typeof(ResponseConvert<string>))]
        public string ApellidoCliente { get; set; }

        [DBFieldDefinition(Name = "NU_COMPROBANTE", ValueConverter = typeof(ResponseConvert<long>))]
        public long NumeroComprobante { get; set; }

        [DBFieldDefinition(Name = "COD_ESTADO_PROCESO", ValueConverter = typeof(ResponseConvert<string>))]
        public string CodEstadoProceso { get; set; }

        [DBFieldDefinition(Name = "DE_FONDO", ValueConverter = typeof(ResponseConvert<string>))]
        public string DescripcionFondo { get; set; }

        [DBFieldDefinition(Name = "CUENTA_TITULO", ValueConverter = typeof(ResponseConvert<long>))]
        public long CuentaTitulo { get; set; }

        [DBFieldDefinition(Name = "CUENTA_OPERATIVA", ValueConverter = typeof(ResponseConvert<long>))]
        public long CuentaOperativa { get; set; }

        [DBFieldDefinition(Name = "FECHA_BAJA", ValueConverter = typeof(ResponseConvert<DateTime>))]
        public DateTime FechaBaja { get; set; }

        [DBFieldDefinition(Name = "CANAL", ValueConverter = typeof(ResponseConvert<string>))]
        public string Canal { get; set; }

        [DBFieldDefinition(Name = "DESTINATION", ValueConverter = typeof(ResponseConvert<string>))]
        public string Destination { get; set; }

        [DBFieldDefinition(Name = "ESTADO_ENVIO", ValueConverter = typeof(ResponseConvert<string>))]
        public string EstadoEnvio { get; set; }

        [DBFieldDefinition(Name = "CANTIDAD_INTENTOS", ValueConverter = typeof(ResponseConvert<long>))]
        public long CantidadIntentos { get; set; }

        [DBFieldDefinition(Name = "FECHA_PEDIDO", ValueConverter = typeof(ResponseConvert<DateTime>))]
        public DateTime FechaPedido { get; set; }

        [DBFieldDefinition(Name = "FECHA_ENVIO", ValueConverter = typeof(ResponseConvert<DateTime>))]
        public DateTime FechaEnvio { get; set; }

        [DBFieldDefinition(Name = "USUARIO", ValueConverter = typeof(ResponseConvert<string>))]
        public string Usuario { get; set; }

        [DBFieldDefinition(Name = "ID_SERVICIO", ValueConverter = typeof(ResponseConvert<string>))]
        public string IdServicio { get; set; }

        [DBFieldDefinition(Name = "OPERACION", ValueConverter = typeof(ResponseConvert<string>))]
        public string Operacion { get; set; }
    }
}
