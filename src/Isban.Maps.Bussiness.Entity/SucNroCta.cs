using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity
{
    [DataContract]
    public class SucNroCta
    {
        [DataMember]
        public long? Sucursal { get; set; }

        /// <summary>
        /// Gets or sets the numero cuenta.
        /// </summary>
        /// <value>
        /// The numero cuenta.
        /// </value>
        [DataMember]
        public long NumeroCuenta { get; set; }
    }
}
