namespace Isban.MapsMB.Entity.Response
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Texto
    {
        [DataMember]
        public string Titulo { get; set; }

        [DataMember]
        public string Asunto { get; set; }

        [DataMember]
        public string TextoMensaje { get; set; }

        [DataMember]
        public string TipoMensaje { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public string TituloSolapa { get; set; }
    }
}
