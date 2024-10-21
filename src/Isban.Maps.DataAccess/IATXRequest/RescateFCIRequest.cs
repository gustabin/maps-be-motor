using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.PDC.Middleware.DataAccess.Requests
{
    public class RescateFCIRequest : BaseIaxtRequest
    {
        public string Cantidad_Cuotas_partes { get; set; }
        public string Codigo_Agente { get; set; }
        public string Codigo_Canal { get; set; }
        public string Codigo_Cliente { get; set; }
        public string Codigo_Fondo { get; set; }
        public string Forma_Pago { get; set; }
        public string Importe_Bruto { get; set; }
        public string Importe_Rescate_Comision { get; set; }
        public string Importe_Rescate_Neto { get; set; }
        public string Impre_solicitud { get; set; }
        public string Moneda { get; set; }
        public string Monto_a_Reversar_KU { get; set; }
        public string Nro_Certif_a_Reversar { get; set; }
        public string Nro_Cuenta { get; set; }
        public string Suc_Cuenta { get; set; }
        public string Terminal_safe { get; set; }
        public string Tipo_Cuenta { get; set; }
    }
}
