namespace Isban.MapsMB.Entity.Response
{
    using Common.Entity;
    using Isban.MapsMB.Entity.Request;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class SolicitudOrden : EntityBase
    {
        [DataMember]
        public CabeceraConsulta Cabecera { get; set; }

        [DataMember]
        public List<CargaSolicitudOrden> Solicitudes { get; set; }
    }
}
