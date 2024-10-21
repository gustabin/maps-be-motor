using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Isban.MapsMB.Common.Entity.Response
{
    /// <summary>
    /// class Entity:  Menu
    /// </summary>

    [DataContract]
    public class SaldoRescatadoResponse
    {
        /// <summary>
        /// Gets or sets the lista saldo rescatado.
        /// </summary>
        /// <value>
        /// The lista saldo rescatado.
        /// </value>
        [DataMember]
        public List<SaldoRescatado> ListaSaldoRescatado { get; set; }

        /// <summary>
        /// Gets or sets the lista errores.
        /// </summary>
        /// <value>
        /// The lista errores.
        /// </value>
        [DataMember]
        public List<ItemError> ListaErrores { get; set; }

        /// <summary>
        /// Errors the unificado.
        /// </summary>
        /// <returns></returns>
        public int ErrorUnificado()
        {
            if (ListaErrores == null || ListaErrores.Count == 0) return 0;
            if ((ListaSaldoRescatado == null || ListaSaldoRescatado.Count == 0) && (ListaErrores != null && ListaErrores.Count > 0)) return 2;
            return 1;
        }
    }

    [DataContract]
    public class ObtenerPasoResponse
    {
    }
}
