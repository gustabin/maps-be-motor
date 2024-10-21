using Isban.MapsMB.Common.Entity.Constantes;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Isban.MapsMB.Common.Entity.Constantes.BusinessConstants;

namespace Isban.MapsMB.Common.Entity.Models
{
    public class Table
    {
        public List<Row> Rows { get; set; }
        public int ColumnCount { get; set; }
        public List<float> RelativeWidths { get; set; }

        public Table()
        {
            Rows = new List<Row>();
            RelativeWidths = new List<float>();
        }

        public Row AddDataRow(List<Cell> values, Font font, bool? bold = null, string border = null)
        {
            //if (evenRow.HasValue && evenRow.Value)
            //{
            //    row.BackgroundColor = Colors.BackgroundColorAlternativeRow;
            //}
            var row = AddRow(values, font, bold, border);
            return row;
        }

        private Row AddRow(List<Cell> values, Font font, bool? bold = null, string border = null)
        {
            var row = new Row();
            values.ForEach(p => { p.Parent = row; p.Border = border; p.Bold = bold; });
            row.Cells.AddRange(values);
            this.Rows.Add(row);
            row.Type = CoreConstants.RowType.Data;
            row.Font = font;
            return row;
        }

        private Row AddRow(List<string> values, Font font, bool? bold = null, string border = null)
        {
            var cells = values.ConvertAll(p => new Cell()
            {
                Value = p,
                Type = CoreConstants.FormatType.String,
                Bold = bold,
                Border = border
            });
            return AddRow(cells, font, bold, border);
        }

        public Row AddTitleRow(List<string> values, Font font)
        {
            var row = AddRow(values, font, (bool?)font.IsBold(), Colors.TableBorder);
            row.Type = CoreConstants.RowType.Header;
            row.BackgroundColor = Colors.BackgroundColorTableHeader;
            return row;
        }

        public Table GetTransposedTable()
        {
            var tTable = new Table();
            tTable.ColumnCount = this.Rows.Count;
            tTable.RelativeWidths = this.RelativeWidths;
            for (int i = 0; i < this.ColumnCount; i++)
            {
                var newCells = new List<Cell>();
                foreach (var row in this.Rows)
                {
                    if (i < row.Cells.Count)
                    {
                        newCells.Add(row.Cells[i]);
                    }
                    else
                    {
                        newCells.Add(new Cell());
                    }
                }
                var newRow = new Row();
                newRow.Cells = newCells;
                tTable.Rows.Add(newRow);
            }
            return tTable;
        }

    }
}
