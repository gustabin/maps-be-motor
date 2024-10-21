using Isban.MapsMB.Common.Entity.Constantes;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Isban.MapsMB.Common.Entity.Constantes.CoreConstants;

namespace Isban.MapsMB.Common.Entity.Models
{
    public class Cell
    {
        public static int Decimales { get; set; }

        public int? Padding { get; set; }


        public FormatType Type { get; set; }

        public bool? Bold { get; set; }

        public object Value { get; set; }

        public bool hasColspan { get; set; }

        public int Colspan { get; set; }

        public bool isEmptyRow { get; set; }

        public string Border { get; set; }

        public Alignment? VerticalAlign { get; set; }

        public Alignment? HorizontalAlign { get; set; }

        public Border BorderLine { get; set; } = CoreConstants.Border.NO_BORDER;

        public float FixedHeight { get; set; }

        public bool HasFixedHeight { get; set; } = false;

        public bool isVerticalTable { get; set; } = false;
        //Inherits from row
        public RowType? ParentRowType { get; set; }

        public string BackGroundColour { get; set; }

        public Font Font { get; set; }

        private static string FormatoMoneda { get; set; }

        private static string FormatoPrecio { get; set; }

        private static string FormatoNumero { get; set; }

        private static string FormatoCuotaparte { get; set; }

        private static string FormatoCantidadCuotaparte { get; set; }

        public Row Parent { get; set; }

        static Cell()
        {
            Decimales = 2;
            FormatoMoneda = "{0:##.##0,00}";
            FormatoNumero = "{0:#,##0.00}";
            FormatoCuotaparte = "{0:#,##0.00000000}";
            FormatoCantidadCuotaparte = "{0:#,##0.000000}";
            FormatoPrecio = "{0:#.##0,0000000}";

        }

        public string GetFormatedValue()
        {
            if (!string.IsNullOrWhiteSpace(Value?.ToString()))
            {
                switch (Type)
                {
                    case FormatType.CurrencyArs:
                        {               
                            return $"{BusinessConstants.SignoMoneda.ARS} {String.Format(FormatoMoneda, string.Format(CultureInfo.GetCultureInfo("es-AR"), "{0:n}", Value))}";
                        }
                    case FormatType.CurrencyUsd:
                        return $"{BusinessConstants.SignoMoneda.USD} {String.Format(FormatoMoneda, string.Format(CultureInfo.GetCultureInfo("es-AR"), "{0:n}", Value))}";
                    case FormatType.CurrencyEur:
                        return $"{BusinessConstants.SignoMoneda.EUR} {String.Format(FormatoMoneda, Value)}";
                    case FormatType.Number:
                        return $"{Math.Round(Convert.ToDecimal(Value), Decimales)}";
                    case FormatType.String:
                        return Value.ToString().Trim();
                    case FormatType.Percentage:
                        return $"{String.Format(FormatoNumero, Value)}%";
                    case FormatType.Date:
                        return ((DateTime)Value).ToString("dd/MM/yyyy");
                    case FormatType.DateTime:
                        return ((DateTime)Value).ToString("dd/MM/yyyy HH:mm");
                    case FormatType.TipoCambio:
                        return Math.Round(Convert.ToDecimal(Value), 4).ToString();
                    case FormatType.Cuotaparte:
                        {
                            var value = double.Parse(Value.ToString());

                            return value.ToString("C4", CultureInfo.GetCultureInfo("es-AR")).Replace("$","");

                            //return String.Format(FormatoMoneda, string.Format(CultureInfo.GetCultureInfo("es-AR"), "{0:n}", Value));
                        }
                    case FormatType.ValorCuotaparte:
                        {
                            var value = double.Parse(Value.ToString());

                            return value.ToString("C6", CultureInfo.GetCultureInfo("es-AR"));

                            //return String.Format(FormatoMoneda, string.Format(CultureInfo.GetCultureInfo("es-AR"), "{0:n}", Value));
                        }
                    case FormatType.CuotaparteArs:
                        {
                            var value = double.Parse(Value.ToString());

                            return value.ToString("C6", CultureInfo.GetCultureInfo("es-AR"));

                            //return String.Format(FormatoMoneda, string.Format(CultureInfo.GetCultureInfo("es-AR"), "{0:n}", Value));
                        }
                    case FormatType.CuotaparteUsd:
                        {
                            var value = double.Parse(Value.ToString());

                            return $"{ BusinessConstants.SignoMoneda.USD} { value.ToString("C6", CultureInfo.GetCultureInfo("es-AR"))}"; 

                            //return String.Format(FormatoMoneda, string.Format(CultureInfo.GetCultureInfo("es-AR"), "{0:n}", Value));
                        }
                    case FormatType.CantidadCuotapartes:
                        return String.Format(FormatoCantidadCuotaparte, Value);
                    case FormatType.Nominales:
                        return $"{String.Format(FormatoNumero, Value)}";
                    case FormatType.PrecioArs:
                        return $"{BusinessConstants.SignoMoneda.ARS} {String.Format(FormatoPrecio, Value)}";
                    case FormatType.PrecioUsd:
                        return $"{BusinessConstants.SignoMoneda.USD} {String.Format(FormatoPrecio, Value)}";
                }
            }
            else
            {
                if (Parent == null) return string.Empty;

                return Parent.Type == RowType.Data ? string.Empty : string.Empty;
            }
            return string.Empty;
        }
    }

}
