using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Response
{
    [DataContract]
    public class CuentaFondoRTF
    {
        [DataMember]
        public long CtaTitulo { get; set; }

        [DataMember]
        public long Sucursal { get; set; }

        [DataMember]
        public string DescripcionCtaTitulo { get; set; }

        [DataMember]
        public string Direccion { get; set; }

        [DataMember]
        public decimal? TotalPesos { get; set; }

        [DataMember]
        public decimal? TotalDolares { get; set; }


        [DataMember]
        public string SimboloPesos { get; set; }

        [DataMember]
        public string SimboloDolares { get; set; }

        [DataMember]
        public Periodo Periodo { get; set; }

        [DataMember]
        public List<EspecieFondoRTF> ListaEspeciesPesos { get; set; }

        [DataMember]
        public List<EspecieFondoRTF> ListaEspeciesDolares { get; set; }

        [DataMember]
        public List<MovimientoEspecieFondoRTF> ListaEspeciesMovimientosPesos { get; set; }

        [DataMember]
        public List<MovimientoEspecieFondoRTF> ListaEspeciesMovimientosDolares { get; set; }

        [DataMember]
        public List<string> Legales { get; set; }
    }
}
