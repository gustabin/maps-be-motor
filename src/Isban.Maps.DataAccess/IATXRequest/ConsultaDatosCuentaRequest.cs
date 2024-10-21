using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.PDC.Middleware.DataAccess.Requests
{
    public class ConsultaDatosCuentaRequest : BaseIaxtRequest
    {
        public string Tipo_Cuenta { get; set; }
		public string Sucursal_Cuenta { get; set; }
		public string Nro_Cuenta { get; set; }

    }
}
