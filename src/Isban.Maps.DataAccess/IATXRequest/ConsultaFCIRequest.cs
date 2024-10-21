namespace Isban.PDC.Middleware.DataAccess.Requests
{
    public class ConsultaFCIRequest : BaseIaxtRequest
    {
        public string Terminal_safe { get; set; }
        public string Codigo_Fondo { get; set; }
        public string Codigo_Cliente { get; set; }
        public string Codigo_Agente { get; set; }
        public string Codigo_Canal { get; set; }
        public string Importe_Bruto { get; set; }
        public string Porcentaje_Comis { get; set; }
        public string Forma_Pago { get; set; }
        public string Nombre_Banco { get; set; }
        public string Numero_Cheque { get; set; }
        public string Tipo_Cuenta { get; set; }
        public string Suc_Cuenta { get; set; }
        public string Nro_Cuenta { get; set; }
        public string Impre_solicitud { get; set; }
        public string Cotizac_Cambio { get; set; }
        public string Fecha_Rescate_futuro { get; set; }
        public string Codigo_Custodia_Actual { get; set; }
        public string Dia_Clearing_Cheques { get; set; }
        public string Moneda { get; set; }

    }
}
