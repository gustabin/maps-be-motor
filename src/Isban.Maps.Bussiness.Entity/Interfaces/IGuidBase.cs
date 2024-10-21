using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Interfaces
{
    public interface IGuidBase
    {
        /// <summary>
        /// Gets or sets the unique identifier error.
        /// </summary>
        /// <value>
        /// The unique identifier error.
        /// </value>
        Guid? GUIDError { get; set; }
    }
}
