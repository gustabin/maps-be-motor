using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;


namespace Isban.MapsMB.Common.Entity.Request
{
    [DataContract]
    public class BajaOperacionReq : EntityBase
    {   
        [DataMember]
        public string IdServicio { get; set; }

        [DataMember]
        public long? IdAdhesion { get; set; }
        
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Dato { get; set; }

        [DataMember]
        public string Firma { get; set; }

        [DataMember]
        public long? IdSimulacion { get; set; }

        [DataMember]
        public string Estado { get; set; }

        [DataMember]
        public virtual List<JObject> Items { get; set; }

        public override bool Validar()
        {
            throw new NotImplementedException();
        }
    }
}


