using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Response
{

    public class AltaCuentaResponse
    {
        public string Código_retorno_extendido { get; set; }

        public string Nro_Cuenta { get; set; }

        public string ID_Sistema { get; set; }

        public string Cant_Desc_Status_Resultado { get; set; }

        public string Descripcion_Status_Resultado { get; set; }
    }

}
