namespace Isban.MapsMB.Entity.Controles.Independientes
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ItemDisclaimer<T>
    {
        [DataMember]
        public int TipoDisclaimer { get; set; }

        [DataMember]
        public T Valor { get; set; }

        [DataMember]
        public string Titulo { get; set; }
    }
}
