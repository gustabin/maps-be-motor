using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Isban.MapsMB.Common.Entity.Constantes.CoreConstants;

namespace Isban.MapsMB.Common.Entity.Models
{
    public class Row
    {
        public Row()
        {
            Cells = new List<Cell>();
        }
        public List<Cell> Cells { get; set; }



        public RowType Type { get; set; }

        public string BackgroundColor { get; set; }

        public Font Font { get; set; }

        public Alignment VerticalAlign { get; set; }

        public Alignment HorizontalAlign { get; set; }

        public void RenderRow()
        {
            if (!string.IsNullOrWhiteSpace(this.BackgroundColor))
            {
                this.Cells.ForEach(c =>
                {
                    c.BackGroundColour = string.IsNullOrWhiteSpace(c.BackGroundColour) ? this.BackgroundColor : c.BackGroundColour;
                });
            }

            if (this.Font != null)
            {
                this.Cells.ForEach(c =>
                {
                    c.Font = c.Font == null ? this.Font : c.Font;
                });
            }

            this.Cells.ForEach(c =>
            {
                c.ParentRowType = c.ParentRowType == null ? this.Type : c.ParentRowType;
            });

            this.Cells.ForEach(c =>
            {
                c.VerticalAlign = c.VerticalAlign == null ? this.VerticalAlign : c.VerticalAlign;
            });

            this.Cells.ForEach(c =>
            {
                c.HorizontalAlign = c.HorizontalAlign == null ? this.HorizontalAlign : c.HorizontalAlign;
            });
        }

    }
}
