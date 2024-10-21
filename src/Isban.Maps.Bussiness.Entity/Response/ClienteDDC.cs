namespace Isban.MapsMB.Entity.Response
{
    using Isban.MapsMB.Common.Entity;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    
    [DataContract]
    public class ClienteDDC : EntityBase
    {   
        [DataMember]
        public string Nombre { get; set; }
  

        [DataMember]
        public string Apellido { get; set; }

        [DataMember]
        public DateTime FechaNacimiento { get; set; }

        [DataMember]
        public string NumeroDocumento { get; set; }
                
        [DataMember]
        public string CodTipoDocumento { get; set; }

        [DataMember]
        public string TipoDocumento { get; set; }

        [DataMember]
        public string SegmentoCuenta { get; set; }

        [DataMember]
        public string TipoBanca { get; set; }

        [DataMember]
        public string Empleado { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public List<ClienteCuentaDDC> Cuentas { get; set; }

        public override bool Validar()
        {
            throw new NotImplementedException();
        }
    }
}