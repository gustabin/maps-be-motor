using System.Runtime.Serialization;

namespace Isban.MapsMB.Common.Entity.Response
{
    public class ControlSimple
    {

        private int _posicion;

        [DataMember]
        public decimal IdComponente { get; set; }

        [DataMember]        
        public decimal? IdPadreDB { get; set; }

        [DataMember]        
        public string Id { get; set; }

        [DataMember]        
        public string Etiqueta { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string Tipo { get; set; }

        [DataMember]        
        public string Ayuda { get; set; }

        [DataMember]        
        public bool Requerido { get; set; }

        [DataMember]        
        public bool Bloqueado { get; set; }

        [DataMember]        
        public int Posicion
        {
            get
            {
                if (_posicion == 0)
                    return 1;
                else
                    return _posicion;
            }
            set
            {
                if (value == 0)
                    _posicion = 1;
                else
                    _posicion = value;
            }
        }

        [DataMember]        
        public bool Validado { get; set; }

        [DataMember]        
        public string Informacion { get; set; }

        [DataMember]        
        public int Error { get; set; }

        [DataMember]        
        public string Error_Desc { get; set; }

        [DataMember]        
        public string Error_Tecnico { get; set; }

        [DataMember]        
        public string Implementa { get; set; }

        [DataMember]        
        public string Config { get; set; }
                           
        //[DataMember]
        //public Cabecera Cabecera { get; set; }
          
}
}