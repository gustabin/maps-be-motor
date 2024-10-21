using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.PDC.Middleware.DataAccess.Requests
{
    public class ConsultaPdcIatxRequest: BaseIaxtRequest
    {
        public string FechaConcertacion { get; set; }
        public string Plazo { get; set; }
        public string TipoCuenta { get; set; }
        public string Sucursal { get; set; }
        public string NumeroCuenta { get; set; }
        public string CuentaTitulos { get; set; }
        public string Operatoria { get; set; }
        public string Segmento { get; set; }

        public void Format()
        {
            Operatoria = Operatoria.PadLeft(20, ' ');
        }
    }
}
