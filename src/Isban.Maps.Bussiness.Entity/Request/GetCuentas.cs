
namespace Isban.MapsMB.Entity.Request
{
    using Isban.Maps.Entity.Base;
    using Isban.MapsMB.Common.Entity;
    using System.Runtime.Serialization;
    using System;

    [DataContract]
    public class GetCuentas : EntityBase
    {                                                                                                
        [DataMember]
        public string DatoConsulta { get; set; }
        
        [DataMember]
        public string TipoBusqueda { get; set; }
        
        [DataMember]
        public string CuentasRespuesta { get; set; }         
        
        [DataMember]
        public string IdServicio { get; set; }

        [DataMember]
        public string Titulares { get; set; }

        [DataMember]
        public CabeceraConsulta Cabecera { get; set; }

        public override bool Validar()
        {
            throw new NotImplementedException();
        }
    }

}
