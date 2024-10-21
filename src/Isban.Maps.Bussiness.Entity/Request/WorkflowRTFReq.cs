using Isban.MapsMB.Common.Entity.Response;
using Isban.MapsMB.Entity.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Request
{
    public class WorkflowRTFReq
    {
        [DataMember]
        public CabeceraConsulta Cabecera { get; set; }

        [DataMember]
        public string IdServicio { get; set; }

        [DataMember]
        public List<CuentasActivasRTF> CuentasActivas { get; set; }

        [DataMember]
        public string Mail { get; set; }

        [DataMember]
        public string RequiereNup { get; set; }

        [DataMember]
        public string Batch { get; set; }

    }
}
