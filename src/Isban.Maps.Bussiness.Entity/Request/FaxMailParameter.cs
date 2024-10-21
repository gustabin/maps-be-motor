using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Request
{
    public class FaxMailParameter
    {
        public string Tipo { get; set; }
        public string Usuario { get; set; }
        public string Emisor { get; set; }
        public string Receptores { get; set; }
        public string Copia { get; set; }
        public string CopiaOculta { get; set; }
        public string ResponderA { get; set; }
        public string Asunto { get; set; }
        public bool EsHtml { get; set; }
        public string Mensaje { get; set; }
        public string NombreAdjunto { get; set; }
        public string TipoAdjunto { get; set; }
        public List<string> Adjuntos { get; set; }
        public bool AcuseRecibo { get; set; }
        public bool AcuseLectura { get; set; }
        public string Prioridad { get; set; }
    }
}
