using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Constantes
{
    public class BusinessConstants
    {
        public enum Products
        {
            PF,
            FCI,
            TITRent,
            TITMov,
            AccDiv,
            AccMov
        }

        public class Segmento
        {
            public const string RTF = "Resumen de Fondos Comunes de Inversión";
        }

        public class DetalleFondos
        {
            public const string Tipo = "TIPO";
            public const string Fondo = "FONDO";
            public const string ValorCuotaparte = "VALOR CUOTAPARTE";
            public const string Cuotapartes = "CUOTAPARTES";
            public const string TenenciaValuada = "TENENCIA VALUADA";
            public const string Fecha = "FECHA";
            public const string Concepto = "CONCEPTO";
            public const string Comprobante = "COMPROBANTE";
            public const string Importe = "IMPORTE NETO";
            public const string TotalFondosPesos = "Total Fondos en Pesos";
            public const string TotalFondosDolares = "Total Fondos en Dólares";            

        }

        public class TipoFondo
        {
            public const string FondoPesos = "Fondos en pesos al";
            public const string FondoDolares = "Fondos en dólares al";
            public const string MovimientosPesos = "Movimientos de los fondos en pesos";
            public const string MovimientosDolares = "Movimientos de los fondos en dólares";
        }

        public class Moneda
        {
            public const string ARS = "ARS";
            public const string USD = "USD";
            public const string EUR = "EUR";
        }

        public class SignoMoneda
        {
            public const string ARS = "$";
            public const string USD = "u$s";
            public const string EUR = "€";
        }

        public class DescMoneda
        {
            public const string ARS = "Pesos";
            public const string USD = "Dólares";
            public const string EUR = "Euros";
        }

        public class Colors
        {
            public const string Font = "#033333";
            public const string FontTableHeader = "#333333";
            public const string BackgroundColorTableHeader = "#E3E3E2";
            public const string BackgroundColorTableData = "#EFEFEF";
            public const string BackgroundColorTableData2 = "#FCFCFC";
            public const string TableBorder = null; //"#CECECE";
            public const string BackgroundColorAlternativeRow = "#EFEEEE";
            public const string LineSeparator = "#9F9F9F";

            public const string BackgroundColorHeaderH3H4 = "#CCCBCA";
            public const string BackgroundColorDataH3H4 = "#E6E6E6";
        }

        public class FontSize
        {
            //Ej: Plazo Fijo Tradicional en Pesos
            public const float TituloSubGrupo = 13;
            public const float TituloEspecie = 8;
            public const float TituloParticipes = 8;

            public const float CabeceraTabla = 8;
            public const float DataTabla = 7;
            public const float DataParticipes = 8;

        }
    }
}
