using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace Isban.MapsMB.Common.Entity.Response
{

    [DataContract]
    public class BajaAdhesionMAPS
    {
        [DataMember]
        public string Codigo { get; set; }
        [DataMember]
        public string Mensaje { get; set; }
        [DataMember]
        public string MensajeTecnico { get; set; }
        [DataMember]
        public BajaDatos Datos { get; set; }

        
    }

    [DataContract]
    public class BajaDatos
    {
        [DataMember]
        public string IdServicio { get; set; }
        [DataMember]
        public string Estado { get; set; }
        [DataMember]
        public string Nup { get; set; }
        [DataMember]
        public long? IdSimulacion { get; set; }
        [DataMember]
        public string Comprobante { get; set; }
        [DataMember]
        public string Segmento { get; set; }
        [DataMember]
        public string Canal { get; set; }
        [DataMember]
        public string SubCanal { get; set; }
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public string Validado { get; set; }
        [DataMember]
        public string Error { get; set; }
        [DataMember]
        public string Error_Desc { get; set; }

    }
}
