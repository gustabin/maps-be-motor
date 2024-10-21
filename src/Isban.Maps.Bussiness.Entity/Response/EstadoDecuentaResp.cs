using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Isban.MapsMB.Entity.Response
{
    [DataContract]
    public class EstadoDecuentaResp
    {
        public EstadoDecuentaResp()
        {
            Activas = new List<CargaSolicitudOrden>();
            Bloqueadas = new List<CargaSolicitudOrden>();
            NoDDC = new List<CargaSolicitudOrden>();
        }

        [DataMember]
        public List<CargaSolicitudOrden> Activas { get; set; }

        [DataMember]
        public List<CargaSolicitudOrden> Bloqueadas { get; set; }

        [DataMember]
        public List<CargaSolicitudOrden> NoDDC { get; set; }
    }

}