using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Response
{

    public class FormularioResponse : ControlSimple
    {
        public FormularioResponse()
        {
            Items = new List<dynamic>();
        }

        [DataMember]
        public string IdServicio { get; set; }

        [DataMember]
        public long? IdSimulacion { get; set; }

        [DataMember]
        public string Comprobante { get; set; }

        [DataMember]
        public string Estado { get; set; }

        [DataMember]
        public string FormAnterior { get; set; }

        [DataMember]
        public long? IdAdhesion { get; set; }

        [DataMember]
        public string Titulo { get; set; }

        [DataMember]
        public string Nup { get; set; }

        [DataMember]
        public string Segmento { get; set; }

        [DataMember]
        public string Canal { get; set; }

        [DataMember]
        public string SubCanal { get; set; }

        [DataMember]
        public string PerfilInversor { get; set; }

        [DataMember]
        public List<dynamic> Items { get; set; }

        [DataMember]
        public string TextoJson { get; set; }

        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string Ip { get; set; }

        [DataMember]
        public decimal? FormularioId { get; set; }

        [DataMember]
        public string ListaCtasOperativas { get; set; }

        [DataMember]
        public string ListaCtasTitulo { get; set; }
    }
}



