using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Response
{
    public class ObtenerCuentasRepaResp
    {
      
        public string Nup { get; set; }


        public long? CodAltaAdhesion { get; set; }



        public long? CuentaTitulo { get; set; }


        public long? CtaOperativa { get; set; }

        public long? SucursalOperativa { get; set; }


        public long? TipoOperativa { get; set; }


        public long? CtaOperativaDolares { get; set; }

        public long? SucursalOperativaDolares { get; set; }


        public long? TipoOperativaDolares { get; set; }
    }
}
