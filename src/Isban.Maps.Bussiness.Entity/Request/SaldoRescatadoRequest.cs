using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Request
{
    [DataContract]
    public class SaldoRescatadoRequest : EntityBase
    {
        public SaldoRescatadoRequest()
        {
            ListaCuentas = new List<SucNroCta>();
        }
        /// <summary>
        /// Gets or sets the lista cuentas.
        /// </summary>
        /// <value>
        /// The lista cuentas.
        /// </value>
        [DataMember]
        public List<SucNroCta> ListaCuentas { get; set; }

        /// <summary>
        /// Gets or sets the codigo fondo.
        /// </summary>
        /// <value>
        /// The codigo fondo.
        /// </value>
        [DataMember]
        public string CodigoFondo { get; set; }

        /// <summary>
        /// Gets or sets the fecha hasta.
        /// </summary>
        /// <value>
        /// The fecha hasta.
        /// </value>
        [DataMember]
        public DateTime FechaLiquidacionHasta { get; set; }

        // <summary>
        /// Gets or sets the datos servicios.
        /// </summary>
        /// <value>
        /// The datos servicios.
        /// </value>
        [DataMember]
        public DatosServicios DatosServicios { get; set; }
       
    }
}
