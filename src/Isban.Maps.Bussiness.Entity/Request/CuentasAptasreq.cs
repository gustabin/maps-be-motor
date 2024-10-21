
namespace Isban.MapsMB.Entity.Request
{
    using Isban.MapsMB.Common.Entity;
    using System.Runtime.Serialization;
    using System;

    [DataContract]
    public class CuentasAptasReq : EntityBase
    {
        [DataMember]
        public string DatoConsulta { get; set; }
        
        [DataMember]
        public string TipoBusqueda { get; set; }
        
        [DataMember]
        public string CuentasRespuesta { get; set; }
        
        [DataMember]
        public string IdServicio { get; set; }
        
        public override bool Validar()
        {
            throw new NotImplementedException();
        }
    }
}
