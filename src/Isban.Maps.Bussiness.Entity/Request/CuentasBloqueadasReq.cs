using Isban.MapsMB.Common.Entity;
using System.Runtime.Serialization;
using System;

namespace Isban.MapsMB.Entity.Request
{
    public class CuentasBloqueadasReq : EntityBase
    {
        [DataMember]
        public string CuentasRespuesta { get; set; }

        [DataMember]
        public string NroCta { get; set; }

        [DataMember]
        public long? IdAdhesion { get; set; }

        [DataMember]
        public string IdServicio { get; set; }

        [DataMember]
        public CabeceraConsulta Cabecera { get; set; }

        public override bool Validar()
        {
            throw new NotImplementedException();
        }
    }
}
