

using System.Runtime.Serialization;

namespace Isban.MapsMB.Common.Entity.Response
{
    [DataContract]
    public class ConsultaLoadAtisResponse 
    {
        [DataMember]
        public long? CuentaBp { get; set; }

        [DataMember]
        public long? CuentaAtit { get; set; }

        /// <summary>
        /// Compara dos valores cualesquiera para ver si son iguales
        /// </summary>
        /// <param name="valor1">String valor 1</param>
        /// <param name="valor2">String valor 2</param>
        public bool Validar(string valor1, string valor2)
        {
            if (valor1.ToLower().Trim().Equals(valor2.ToLower().Trim()))
            {
                return true;

            }

            return false;
        }
    }
}
