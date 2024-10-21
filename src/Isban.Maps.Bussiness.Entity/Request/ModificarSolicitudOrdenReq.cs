using Isban.MapsMB.Common.Entity;
using System.Runtime.Serialization;
using System;

namespace Isban.MapsMB.Entity.Request
{
    public class ModificarSolicitudOrdenReq : EntityBase
    {
        [DataMember]
        public long IdSolicitudOrdenes { get; set; }

        [DataMember]
        public string Estado { get; set; }

        [DataMember]
        public long NumOrdenOrigen { get; set; }

        [DataMember]
        public string Observacion { get; set; }

        [DataMember]
        public long? TipoDisclaimer { get; set; }

        [DataMember]
        public string TextoDisclaimer { get; set; }

        [DataMember]
        public long IdEvaluacion { get; set; }

        [DataMember]
        public decimal? SaldoEnviado { get; set; }

        [DataMember]
        public long? CodBajaAdhesion { get; set; }

        [DataMember]
        public decimal? SaldoCuentaAntes { get; set; }

        [DataMember]
        public decimal? SaldoMinPorFondo { get; set; }

        [DataMember]
        public decimal? SaldoRescatePorFondo { get; set; }

        [DataMember]
        public string Comprobantes { get; set; } // Solo se utiliza para generar bajas masivas ---> NO BORRAR

        public override bool Validar()
        {
            throw new NotImplementedException();
        }
    }
}
