using Isban.MapsMB.Entity.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Request
{
    public class RTFWorkflowOnDemandReq
    {
        [DataMember]
        public CabeceraConsulta Cabecera { get; set; }

        [DataMember]
        public string Nup { get; set; }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string CuentaTitulo { get; set; }
    }
}
