using Isban.Mercados;
using Isban.Mercados.Security.Adsec.Entity;

namespace Isban.MapsMB.Common.Entity
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class EntityBase : BaseValidacion, ITraceEntity
    {
        [DataMember]
        public System.Guid? IdTrace { get; set; }

        [DataMember]
        public string Canal { get; set; }

        [DataMember]
        public string SubCanal { get; set; }

        [DataMember]
        public string Nup { get; set; }

        [DataMember]
        public string Segmento { get; set; }

        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string Ip { get; set; }

        public override bool Validar()
        {
            return true;
        }
    }

    [DataContract]
    public class DatoFirmaMaps
    {
        #region atributos
        /// <summary>
        /// Tipo de Hash a utilizar en la firma B64, PEM
        /// </summary>
        [DataMember]
        public TipoHash TipoHash { get; set; }
        /// <summary>
        /// Dato Firmados
        /// </summary>
        [DataMember]
        public string Firma { get; set; }
        /// <summary>
        /// Dato
        /// </summary>
        [DataMember]
        public string Dato { get; set; }
        /// <summary>
        /// Informa si los datos viene en la firma
        /// </summary>
        [DataMember]
        public DatosFirmado DatosFirmado { get; set; }
        #endregion
    }
}
