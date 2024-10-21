using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Response
{
    public class ResultadoTransferencia
    {
        public ResultadoTransferencia()
        {
            Ok = new List<DatosTransferencia>();
            NoOk = new List<DatosTransferencia>();
        }

        public List<DatosTransferencia> Ok { get; set; }
        public List<DatosTransferencia> NoOk { get; set; }
    }
}
