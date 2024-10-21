using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Response
{
    public class MotorResponse
    {
        public bool Exitoso { get; set; }
        public string Descripcion { get; set; }
        public DateTime InicioEjecución { get; set; }
        public DateTime FinEjecución { get; set; }
        public TimeSpan EjecucionTotal { get; set; }

        public dynamic ResultadoServicio { get; set; }
    }
}
