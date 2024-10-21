using System;
using System.Runtime.Serialization;
using Isban.MapsMB.Common.Entity;

namespace Isban.PDC.Middleware.Entity
{
    [DataContract]
    public abstract class BaseIatx : BaseValidacion
    {
        #region Propiedades
        [DataMember]
        public string CanalTipo { get; set; }
        [DataMember]
        public string SubCanalId { get; set; }
        [DataMember]
        public string CanalVer { get; set; }
        [DataMember]
        public string SubcanalTipo { get; set; }
        [DataMember]
        public string CanalId { get; set; }
        [DataMember]
        public string UsuarioTipo { get; set; }
        [DataMember]
        public string UsuarioId { get; set; }
        [DataMember]
        public string UsuarioAttr { get; set; }
        [DataMember]
        public string UsuarioPwd { get; set; }
        [DataMember]
        public string IdusConc { get; set; }
        [DataMember]
        public string NumSec { get; set; }
        [DataMember]
        public string Nup { get; set; }
        [DataMember]
        public string TipoCliente { get; set; }
        [DataMember]
        public string TipoIdCliente { get; set; }
        [DataMember]
        public string NroIdCliente { get; set; }
        [DataMember]
        public string FechaNac { get; set; }
        [DataMember]
        public string IndSincro { get; set; }

        //[DataMember]
        //public string NroDni { get; set; }
        //[DataMember]
        //public string Subcanal { get; set; }
        //[DataMember]
        //public string TipoDni { get; set; }
        //[DataMember]
        //public string Usuario { get; set; }
        //[DataMember]
        //public string Password { get; set; }
        //[DataMember]
        //public string Ip { get; set; }
        //[DataMember]
        //public string Canal { get; set; }
        #endregion

        public BaseIatx()
        {
            //Cabecera
            CanalId = new string('0', 4);
            CanalVer = new string('0', 3);
            UsuarioAttr = new string('0', 2);
            IdusConc = new string('0', 8); ;
            NumSec = new string('0', 8); ;
            //Subcanal = "";
            SubcanalTipo = new string('0', 2);
            IndSincro = new string('1', 1);
        }

        public override bool Validar()
        {
            //ValidarLargoRango("Canal", Canal, 0, 2);
            ValidarLargo("CanalId", CanalId, 4);
            ValidarLargo("CanalVer", CanalVer, 3);
            ValidarLargo("SubcanalTipo", SubcanalTipo, 2);
            //            ValidarLargo("Subcanal", Subcanal, 4);
            ValidarLargo("UsuarioTipo", UsuarioTipo, 2);
            ValidarLargoRango("UsuarioId", UsuarioId, 0, 8);
            ValidarLargo("UsuarioAttr", UsuarioAttr, 2);
            ValidarLargoRango("UsuarioPwd", UsuarioPwd, 0, 8);
            ValidarLargo("IdusConc", IdusConc, 8);
            ValidarLargo("NumSec", NumSec, 8);
            ValidarLargoRango("Nup", Nup, 0, 8);
            ValidarLargoRango("TipoCliente", TipoCliente, 0, 1);
            //            ValidarLargoRango("TipoDni", TipoDni, 0, 1);
            //            ValidarLargoRango("NroDni", NroDni, 0, 11);
            ValidarLargoRango("FechaNac", FechaNac, 0, 8);

            if (!EsNumerico(FechaNac))
            {
                Errores += ($"FechaNac debe ser numérico (8 caracteres).\r\n");
                TieneErrores = true;
            }

            ValidarContenido("TipoCliente", TipoCliente, "I", "E");
            //            ValidarContenido("TipoDni", TipoDni, "N", "L", "P");

            this.ValidarDatos();

            if (TieneErrores)
            {
                throw new Exception(Errores);
            }
            return true;
        }

        protected abstract void ValidarDatos();
    }
}