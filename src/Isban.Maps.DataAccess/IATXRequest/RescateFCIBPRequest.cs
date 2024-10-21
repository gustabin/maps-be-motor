using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.PDC.Middleware.DataAccess.Requests
{
    public class RescateFCIBPRequest : BaseIaxtRequest
    {
        public string Tipo_transaccion { get; set; }
        public string Codigo_Fondo { get; set; }
        public string Codigo_Cliente { get; set; }
        public string Codigo_Agente { get; set; }
        public string Codigo_Canal { get; set; }
        public string Nro_Certificado { get; set; }
        public string Importe_Neto { get; set; }
        public string Cantidad_Cuotas_partes { get; set; }
        public string Porcentaje_comision { get; set; }
        public string Forma_Pago { get; set; }
        public string Impre_Solicitud { get; set; }
        public string Codigo_Custodia { get; set; }
        public string Nro_Certif_a_Reversar { get; set; }
    }
}
