using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity
{
    [DataContract]
    public class DatosServicios
    {

        /// <summary>
        /// Gets or sets the canal tipo.
        /// </summary>
        /// <value>
        /// The canal tipo.
        /// </value>
        [DataMember]
        public string CanalTipo { get; set; }

        /// <summary>
        /// Gets or sets the canal identifier.
        /// </summary>
        /// <value>
        /// The canal identifier.
        /// </value>
        [DataMember]
        public string CanalId { get; set; }

        /// <summary>
        /// Gets or sets the canal version.
        /// </summary>
        /// <value>
        /// The canal version.
        /// </value>
        [DataMember]
        public string CanalVersion { get; set; }

        /// <summary>
        /// Gets or sets the subcanal tipo.
        /// </summary>
        /// <value>
        /// The subcanal tipo.
        /// </value>
        [DataMember]
        public string SubcanalTipo { get; set; }

        /// <summary>
        /// Gets or sets the subcanal identifier.
        /// </summary>
        /// <value>
        /// The subcanal identifier.
        /// </value>
        [DataMember]
        public string SubcanalId { get; set; }

        /// <summary>
        /// Gets or sets the usuario tipo.
        /// </summary>
        /// <value>
        /// The usuario tipo.
        /// </value>
        [DataMember]
        public string UsuarioTipo { get; set; }

        /// <summary>
        /// Gets or sets the usuario identifier.
        /// </summary>
        /// <value>
        /// The usuario identifier.
        /// </value>
        [DataMember]
        public string UsuarioId { get; set; }

        /// <summary>
        /// Gets or sets the usuario atrib.
        /// </summary>
        /// <value>
        /// The usuario atrib.
        /// </value>
        [DataMember]
        public string UsuarioAtrib { get; set; }

        /// <summary>
        /// Gets or sets the usuario password.
        /// </summary>
        /// <value>
        /// The usuario password.
        /// </value>
        [DataMember]
        public string UsuarioPwd { get; set; }

        /// <summary>
        /// Gets or sets the tipo persona.
        /// </summary>
        /// <value>
        /// The tipo persona.
        /// </value>
        [DataMember]
        public string TipoPersona { get; set; }

        /// <summary>
        /// Gets or sets the tipo identifier.
        /// </summary>
        /// <value>
        /// The tipo identifier.
        /// </value>
        [DataMember]
        public string TipoId { get; set; }

        /// <summary>
        /// Gets or sets the identifier cliente.
        /// </summary>
        /// <value>
        /// The identifier cliente.
        /// </value>
        [DataMember]
        public string IdCliente { get; set; }

        /// <summary>
        /// Gets or sets the fecha nac.
        /// </summary>
        /// <value>
        /// The fecha nac.
        /// </value>
        [DataMember]
        public string FechaNac { get; set; }
                
    }
}
