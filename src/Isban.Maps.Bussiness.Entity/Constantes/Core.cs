using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Constantes
{
    public class CoreConstants
    {
        public enum FormatType
        {
            String,
            CurrencyArs,
            CurrencyUsd,
            Number,
            Percentage,
            DateTime,
            Date,
            TipoCambio,
            Cuotaparte,
            CuotaparteArs,
            CuotaparteUsd,
            CantidadCuotapartes,
            Nominales,
            PrecioArs,
            PrecioUsd,
            CurrencyEur,
            ValorCuotaparte
        }

        public enum RowType
        {
            Data,
            Header,
            Footer
        }

        public enum TableType
        {
            Vertical,
            Horizontal1,
            Horizontal2,
            Horizontal3,
            Horizontal4
        }

        public enum Alignment : int
        {
            TOP = 4,
            MIDDLE = 5,
            BOTTOM = 6,
            LEFT = 0,
            CENTER = 1,
            RIGHT = 2
        }

        public enum Border : int
        {
            TOP = 1,
            BOTTOM = 2,
            LEFT = 4,
            RIGHT = 8,
            NO_BORDER = 0
        }
    }
}
