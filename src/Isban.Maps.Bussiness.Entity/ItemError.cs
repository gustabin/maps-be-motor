using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity
{
    /// <summary>
    /// class ItemError
    /// </summary>
    [DebuggerDisplay("Error: {GetShortError()}")]
    public class ItemError
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        [DataMember]
        public Guid? GUID { get; set; }
        /// <summary>
        /// Gets or sets the fecha error.
        /// </summary>
        /// <value>
        /// The fecha error.
        /// </value>
        [DataMember]
        public DateTime FechaError { get; set; }
        /// <summary>
        /// Gets or sets the mensaje tecnico.
        /// </summary>
        /// <value>
        /// The mensaje tecnico.
        /// </value>
        [DataMember]
        public string MensajeTecnico { get; set; }
        /// <summary>
        /// Gets or sets the codigo.
        /// </summary>
        /// <value>
        /// The codigo.
        /// </value>
        [DataMember]
        public long Codigo { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemError"/> class.
        /// </summary>
        public ItemError()
        {
            Codigo = -1;
        }

        /// <summary>
        /// Gets the short error.
        /// </summary>
        /// <returns></returns>
        private string GetShortError()
        {
            var replace = "Isban.Mercados.DBException: Error no controlado en Acceso a Datos. --->";
            return MensajeTecnico.Replace(replace, "");
        }
    }
}
