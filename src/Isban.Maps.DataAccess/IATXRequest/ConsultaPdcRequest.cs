using System.Runtime.Serialization;

namespace Isban.PDC.Middleware.Entity
{
    public class ConsultaPdcRequest : BaseIatx
    {
        [DataMember]
        public string FechaConcertacion { get; set; }
        [DataMember]
        public long Plazo { get; set; }
        [DataMember]
        public long TipoCtaOper { get; set; }
        [DataMember]
        public string SucCtaOper { get; set; }
        [DataMember]
        public string NumeroCuenta { get; set; }
        [DataMember]
        public long CuentaTitulos { get; set; }
        [DataMember]
        public string TipoEspecie { get; set; }
        [DataMember]
        public string Segmento { get; set; }

        [DataMember]
        public string Usuario { get; set; }
        [DataMember]
        public string Ip { get; set; }

        protected override void ValidarDatos()
        {
            if (!EsNumerico(FechaConcertacion))
            {
                Errores += ($"FechaConcertacion debe ser numérico (8 caracteres).\r\n");
                TieneErrores = true;
            }

            ValidarLargo("Plazo", Plazo, 3);
            ValidarLargo("SucCtaOper", SucCtaOper, 3);
            ValidarLargo("NumeroCuenta", NumeroCuenta, 7);
            ValidarLargo("CuentaTitulos", CuentaTitulos, 10);
            ValidarLargo("Operatoria", Segmento, 20);
                        
        }
    }
}
