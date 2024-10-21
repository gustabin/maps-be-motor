using Isban.Common.Data;
using Isban.Common.Data.Providers.IATX;

namespace Isban.PDC.Middleware.DataAccess.Response
{
    [IATXCollectionDefinition(TableName = "Output")]
    public class SuscripcionFCIResponse : Isban.Common.Data.IContract
    {
        [DBFieldDefinition(Name = "Status_Resultado_Extendido")]
        public string Status_Resultado_Extendido { get; set; }
        [DBFieldDefinition(Name = "Descripcion_Moneda")]
        public string Descripcion_Moneda { get; set; }
        [DBFieldDefinition(Name = "Numero_Certificado")]
        public string Numero_Certificado { get; set; }
        [DBFieldDefinition(Name = "Importe_Neto")]
        public string Importe_Neto { get; set; }
        [DBFieldDefinition(Name = "Porcentaje_Comis")]
        public string Porcentaje_Comis { get; set; }
        [DBFieldDefinition(Name = "Valor_Comision")]
        public string Valor_Comision { get; set; }
        [DBFieldDefinition(Name = "Estado_Actual")]
        public string Estado_Actual { get; set; }
        [DBFieldDefinition(Name = "Motivo_Actual")]
        public string Motivo_Actual { get; set; }
        [DBFieldDefinition(Name = "Cotizac_Cambio")]
        public string Cotizac_Cambio { get; set; }
        [DBFieldDefinition(Name = "Dias_Carencia")]
        public string Dias_Carencia { get; set; }
        [DBFieldDefinition(Name = "Nombre_fondo")]
        public string Nombre_fondo { get; set; }
        [DBFieldDefinition(Name = "Importe_moneda_fondo")]
        public string Importe_moneda_fondo { get; set; }
        //[DBFieldDefinition(Name = "ID_Sistema")]
        //public string ID_Sistema { get; set; }
        //[DBFieldDefinition(Name = "Cant_Desc_Status_Resultado")]
        //public string Cant_Desc_Status_Resultado { get; set; }
        //[DBFieldDefinition(Name = "Descripcion_Status_Resultado")]
        //public string Descripcion_Status_Resultado { get; set; }
        //        <ID_Sistema></ID_Sistema>
        //<Cant_Desc_Status_Resultado></Cant_Desc_Status_Resultado>
        //<Descripcion_Status_Resultado></Descripcion_Status_Resultado>
    }

    [IATXCollectionDefinition(TableName = "Output")]
    public class SuscripcionFCIOBEResponse : Isban.Common.Data.IContract
    {
        //[DBFieldDefinition(Name = "Status_Resultado_Extendido")]
        //public string Status_Resultado_Extendido { get; set; }
     
    }
}