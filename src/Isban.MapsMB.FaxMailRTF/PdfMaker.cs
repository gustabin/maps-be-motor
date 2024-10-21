using Isban.MapsMB.Common.Entity.Constantes;
using Isban.MapsMB.Common.Entity.Models;
using Isban.MapsMB.Common.Entity.Response;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static Isban.MapsMB.Common.Entity.Constantes.BusinessConstants;
using static iTextSharp.text.Font;

namespace Isban.MapsMB.FaxMailRTF
{
    public class PdfMaker
    {
        //static Image redSquare = Image.GetInstance("./Images/cuadradito.png");
        static Font regularFont = FontFactory.GetFont("open sans", 9, Font.NORMAL, BaseColor.BLACK);

        public Document CreateDocument(string pageSize)
        {
            var doc = new Document(PageSize.A4);
            doc.AddTitle("Resumen Trimestral de Fondos");
            doc.AddCreator("Santander Tec");
            doc.SetMargins(35, 35, 102, 115);
            return doc;
        }

        public PdfWriter InitializeDocument(Document doc, string path,string numInversor,Periodo periodo, string nombre, string direccion,string nombresClientes)
        {
            var writer = PdfWriter.GetInstance(doc,
                            new FileStream(path, FileMode.Create));

            writer.PageEvent = new HeaderFooter(numInversor, nombre, direccion, periodo,nombresClientes);
            doc.Open();

            return writer;
        }


        public void AddPageCount(Document doc, string path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            Font blackFont = FontFactory.GetFont("OpenSans-Light", 8, Font.BOLD, BaseColor.BLACK);
            using (MemoryStream stream = new MemoryStream())
            {
                PdfReader reader = new PdfReader(bytes);
                using (PdfStamper stamper = new PdfStamper(reader, stream))
                {
                    int pages = reader.NumberOfPages;
                    for (int i = 1; i <= pages; i++)
                    {
                        ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_CENTER, new Phrase(i.ToString() + "/" + pages, blackFont), 310, 40, 0);
                    }
                }
                bytes = stream.ToArray();
            }
            File.WriteAllBytes(path, bytes);
        }

        public bool AddItem(Document doc, IElement item)
        {
            return doc.Add(item);

        }

        public bool InsertNewPage(Document doc)
        {
            return doc.NewPage();
        }

        public PdfPCell MakeCell(Cell coreCell)
        {
            if (coreCell == null)
            {
                return SetCellStyle(new Cell(), new PdfPCell());
            }

            var font = coreCell.Font ?? GetRegularFont();
            Font emptyFont = FontFactory.GetFont("open sans", 9, Font.NORMAL, BaseColor.WHITE);
   
            var pdfCell = new PdfPCell(new Phrase(coreCell.GetFormatedValue(), coreCell.isEmptyRow ? emptyFont : font));
  
            return SetCellStyle(coreCell, pdfCell);
        }

        public PdfPRow MakeRow(Row coreRow, int ce)
        {

            var pdfRow = new PdfPCell[ce];
            coreRow.RenderRow();
            var cells = coreRow.Cells.ToArray();
            for (int i = 0; i < ce; i++)
            {
                PdfPCell pdfCell;
                if (cells.Length > i)
                {
                    pdfCell = MakeCell(cells[i]);
                }
                else
                {
                    pdfCell = MakeCell(null);
                }
                pdfRow[i] = pdfCell;
            }

            return new PdfPRow(pdfRow);
        }

        public PdfPTable MakeTable(Table coreTable, int? spacing)
        {
            var pdfTable = new PdfPTable(coreTable.ColumnCount);
            foreach (var coreRow in coreTable.Rows)
            {
                var pdfRow = MakeRow(coreRow, coreTable.ColumnCount);
                pdfTable.Rows.Add(pdfRow);
            }
            pdfTable.SpacingBefore = spacing ?? 0;
            if (coreTable.RelativeWidths != null && coreTable.RelativeWidths.Count > 0)
            {
                pdfTable.SetWidths(coreTable.RelativeWidths.ToArray());

            }

            //pdfTable.PaddingTop = 300;
            //pdfTable.SetExtendLastRow(false, false);
            return pdfTable;
        }

        public IElement MakeTitle(string text, Font font, bool centered, int? spacing)
        {
            var title = new Paragraph(text, font);
            if (centered)
            {
                title.Alignment = Element.ALIGN_CENTER;
            }
            title.SpacingBefore = spacing ?? 0;
            return title;
        }

        public PdfPTable MakeGroupTitle(string text, string subtext, Font font, Font subTextFont, int? spacingBefore)
        {
            var tbHeader = new PdfPTable(2);
            tbHeader.WidthPercentage = 100;
            tbHeader.HorizontalAlignment = Element.ALIGN_LEFT;

            var cell = new PdfPCell();
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.FixedHeight = 28;
            cell.PaddingBottom = 0;
            cell.PaddingRight = 0;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.Border = 0;

            var title = new Paragraph(text, font);

            var cellTitle = new PdfPCell(title);
            cellTitle.HorizontalAlignment = Element.ALIGN_LEFT;
            cellTitle.PaddingTop = 5;
            cellTitle.VerticalAlignment = Element.ALIGN_CENTER;
            cellTitle.Border = Rectangle.NO_BORDER;

            tbHeader.SetWidths(new float[] { 3, 99 });
            tbHeader.SpacingBefore = spacingBefore ?? 0;

            if (!string.IsNullOrEmpty(subtext))
            {
                var subTextCell = new PdfPCell(new Phrase(subtext, subTextFont ?? GetRegularFont()));
                subTextCell.HorizontalAlignment = Element.ALIGN_LEFT;
                subTextCell.PaddingTop = 2;
                subTextCell.Border = Rectangle.NO_BORDER;

                cell.Rowspan = 2;
                cellTitle.PaddingTop = 2;

                tbHeader.AddCell(cell);
                tbHeader.AddCell(cellTitle);
                tbHeader.AddCell(subTextCell);

                return tbHeader;
            }

            tbHeader.AddCell(cell);
            tbHeader.AddCell(cellTitle);
            return tbHeader;
        }

        public IElement MakePFTitle(PdfPTable groupTitle, string resBrutoMoneda, string resBrutoPesos)
        {
            var pfTitleTb = new PdfPTable(2);
            pfTitleTb.WidthPercentage = 100;
            pfTitleTb.SetWidths(new float[] { 58, 42 });

            var leftCell = new PdfPCell(groupTitle);
            leftCell.HorizontalAlignment = Element.ALIGN_LEFT;
            leftCell.Border = 0;
            leftCell.VerticalAlignment = Element.ALIGN_MIDDLE;

            var rightCell = new PdfPCell(MakeResultadoBrutoTable(resBrutoMoneda, resBrutoPesos));
            rightCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rightCell.Border = 0;
            rightCell.VerticalAlignment = Element.ALIGN_MIDDLE;

            pfTitleTb.AddCell(leftCell);
            pfTitleTb.AddCell(rightCell);
            pfTitleTb.SpacingBefore = 20;
            return pfTitleTb;
        }

        public PdfPTable MakeResultadoBrutoTable(string resBrutoMoneda, string resBrutoPesos)
        {
            var tbResBruto = new PdfPTable(2);
            tbResBruto.WidthPercentage = 100;
            tbResBruto.SetWidths(new float[] { 60, 40 });

            var titleMonedaCell = new PdfPCell(new Phrase("Resultado bruto en moneda de Plazo Fijo", GetRegularFont())); //Cambiar Font
            titleMonedaCell.BackgroundColor = GetBaseColour(Colors.BackgroundColorTableHeader);
            titleMonedaCell.Border = Rectangle.BOTTOM_BORDER;
            titleMonedaCell.BorderWidthBottom = 1;
            titleMonedaCell.BorderColorBottom = BaseColor.WHITE;
            titleMonedaCell.PaddingLeft = 8;
            titleMonedaCell.PaddingBottom = 6;
            titleMonedaCell.PaddingTop = 3;
            titleMonedaCell.HorizontalAlignment = Element.ALIGN_LEFT;
            titleMonedaCell.VerticalAlignment = Element.ALIGN_MIDDLE;

            var importeMonedaCell = new PdfPCell(new Phrase(resBrutoMoneda, GetRegularFont()));
            importeMonedaCell.BackgroundColor = GetBaseColour(Colors.BackgroundColorTableHeader);
            importeMonedaCell.Border = Rectangle.BOTTOM_BORDER;
            importeMonedaCell.BorderWidthBottom = 1;
            importeMonedaCell.BorderColorBottom = BaseColor.WHITE;
            importeMonedaCell.PaddingRight = 8;
            importeMonedaCell.PaddingBottom = 6;
            importeMonedaCell.PaddingTop = 3;
            importeMonedaCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            importeMonedaCell.VerticalAlignment = Element.ALIGN_MIDDLE;

            tbResBruto.AddCell(titleMonedaCell);
            tbResBruto.AddCell(importeMonedaCell);

            if (!string.IsNullOrEmpty(resBrutoPesos))
            {
                var titlePesosCell = new PdfPCell(new Phrase("Resultado bruto en pesos", GetRegularFont())); //Cambiar Font
                titlePesosCell.BackgroundColor = GetBaseColour(Colors.BackgroundColorTableHeader);
                titlePesosCell.Border = Rectangle.NO_BORDER;
                titlePesosCell.PaddingLeft = 8;
                titlePesosCell.PaddingBottom = 6;
                titlePesosCell.PaddingTop = 3;
                titlePesosCell.HorizontalAlignment = Element.ALIGN_LEFT;
                titlePesosCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                var importePesosCell = new PdfPCell(new Phrase(resBrutoPesos, GetRegularFont()));
                importePesosCell.BackgroundColor = GetBaseColour(Colors.BackgroundColorTableHeader);
                importePesosCell.Border = Rectangle.NO_BORDER;
                importePesosCell.PaddingRight = 8;
                importePesosCell.PaddingBottom = 6;
                importePesosCell.PaddingTop = 3;
                importePesosCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                importePesosCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbResBruto.AddCell(titlePesosCell);
                tbResBruto.AddCell(importePesosCell);
            }

            return tbResBruto;
        }
        public Font GetFont(float size, bool bold, string colour)
        {
            var fontStyle = bold ? Font.BOLD : Font.NORMAL;
            var fontColour = GetBaseColour(colour);
            var font = new Font(FontFactory.GetFont("open sans", size, fontStyle, fontColour));
            return font;
        }

        public Font GetFontStyle(string style,float size, bool bold, string colour)
        {
            var fontStyle = bold ? Font.BOLD : Font.NORMAL;
            var fontColour = GetBaseColour(colour);
            var font = new Font(FontFactory.GetFont(style, size, fontStyle, fontColour));
            return font;
        }
        private BaseColor GetBaseColour(string code)
        {
            var color = System.Drawing.ColorTranslator.FromHtml(code);
            return new BaseColor(color);
        }

        public void CloseDocument(Document doc, PdfWriter writer)
        {
            doc.Close();
            writer.Close();
        }

        public IDrawInterface MakeSeparator()
        {
            var color = GetBaseColour(Colors.LineSeparator);
            var line = new DottedLineSeparator() { LineWidth = 1f, Percentage = 100, LineColor = color, Alignment = Element.ALIGN_CENTER, Offset = 1, Gap = 5 };
            return line;
        }

        public PdfPCell SetCellStyle(Cell coreCell, PdfPCell pdfCell)
        {
            pdfCell.PaddingLeft = (coreCell.isVerticalTable && coreCell.HorizontalAlign == CoreConstants.Alignment.LEFT) ? 10 : 2;
            pdfCell.PaddingRight = (coreCell.isVerticalTable && coreCell.HorizontalAlign == CoreConstants.Alignment.RIGHT) ? 10 : 2;
            pdfCell.PaddingBottom = coreCell.isVerticalTable ? 0 : 10;
            pdfCell.PaddingTop = coreCell.isVerticalTable ? 0 : 10;

            if (coreCell.Bold == true)
            {
                pdfCell.Phrase.Font.SetStyle(Font.BOLD);
            }
            if (coreCell.Bold == false)
            {
                pdfCell.Phrase.Font.SetStyle(Font.NORMAL);
            }

            if (!string.IsNullOrWhiteSpace(coreCell.BackGroundColour))
            {
                pdfCell.BackgroundColor = GetBaseColour(coreCell.BackGroundColour);
            }

            //Bordes
            pdfCell.Border = (int)coreCell.BorderLine;

            var color = GetBaseColour(Colors.LineSeparator);

            if (pdfCell.Border != 0)
            {
                pdfCell.BorderColor = color;
            }
            else
            {
                pdfCell.BorderColor = BaseColor.WHITE;
            }

            //Chunk linebreak = new Chunk(MakeSeparator());
            //pdfCell.AddElement(linebreak);
            //if (!string.IsNullOrWhiteSpace(coreCell.Border))
            //{
            //    pdfCell.BorderColor = color;
            //}
            //else
            //{
            //    pdfCell.BorderColor = BaseColor.WHITE;
            //}

            pdfCell.VerticalAlignment = (int?)coreCell.VerticalAlign ?? Element.ALIGN_MIDDLE;
            pdfCell.HorizontalAlignment = (int?)coreCell.HorizontalAlign ?? Element.ALIGN_CENTER;

            pdfCell.PaddingRight = coreCell.Padding ?? 0;



            if (coreCell.HasFixedHeight)
            {
                pdfCell.FixedHeight = coreCell.FixedHeight;
            }

            if (coreCell.hasColspan) pdfCell.Colspan = coreCell.Colspan;

            pdfCell.PaddingLeft = 10;

            return pdfCell;
        }

        private Font GetRegularFont()
        {
            return new Font(regularFont);
        }

        public PdfPTable MakeComposeTable(Table vTable, List<Table> hTables)
        {
            float verticalTableHeight = 0;
            var pdfHTables = new List<PdfPTable>();

            foreach (var hTable in hTables)
            {
                hTable.Rows.ForEach(r => r.RenderRow());
                hTable.Rows.ForEach(r => r.Cells.ForEach(c => c.BorderLine = CoreConstants.Border.LEFT | CoreConstants.Border.RIGHT));

                var pdfHTable = MakeTable(hTable, null);
                pdfHTable.TotalWidth = 610f;
                verticalTableHeight += pdfHTable.TotalHeight;
                pdfHTables.Add(pdfHTable);
            }

            var pdfVTable = MakeVerticalTable(vTable, null, verticalTableHeight);

            var composeTable = new PdfPTable(2);
            //VERTICAL CELL
            var verticalCell = new PdfPCell(pdfVTable);
            verticalCell.Rowspan = hTables.Count;
            verticalCell.Border = Rectangle.NO_BORDER;
            verticalCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            verticalCell.HorizontalAlignment = Element.ALIGN_CENTER;
            composeTable.AddCell(verticalCell);

            foreach (var pdfHTable in pdfHTables)
            {
                var hCell = new PdfPCell(pdfHTable);
                hCell.Border = Rectangle.NO_BORDER;
                hCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                composeTable.AddCell(hCell);
            }

            composeTable.SpacingBefore = 15;
            composeTable.SetWidths(new float[] { 1.1f, 3 });
            composeTable.KeepTogether = true;

            return composeTable;
        }

        public PdfPTable MakeVerticalTable(Table table, int? spacing, float totalHeight)
        {
            table.Rows.ForEach(r => r.RenderRow());
            var vTable = table.GetTransposedTable();

            vTable.Rows.ForEach(r => r.Cells.ForEach(c => {
                c.FixedHeight = (totalHeight / vTable.Rows.Count());
                c.HasFixedHeight = true;
            }));
            vTable.Rows.ForEach(r => r.Cells.ForEach(c => c.isVerticalTable = true));
            vTable.Rows.ForEach(r => r.Cells.ForEach(c => c.VerticalAlign = CoreConstants.Alignment.MIDDLE));
            var pdfTable = MakeTable(vTable, spacing);
            pdfTable.SpacingAfter = 0;
            pdfTable.SpacingBefore = 0;
            pdfTable.SetExtendLastRow(true, true);
            return pdfTable;
        }

        public PdfPTable MakeResultadoBruto(string resultado, string expresadoEn)
        {
            var result = new PdfPTable(2);
            var tit = new PdfPCell(new Phrase($"Resultado bruto en {expresadoEn}", GetFont(9, true, (BusinessConstants.Colors.Font))));
            tit.Border = Rectangle.NO_BORDER;
            tit.BackgroundColor = GetBaseColour(Colors.BackgroundColorTableHeader);

            var importe = new PdfPCell(new Phrase(resultado, GetFont(9, true, Colors.Font)));
            importe.Border = Rectangle.NO_BORDER;
            importe.BackgroundColor = GetBaseColour(Colors.BackgroundColorTableHeader);
            importe.HorizontalAlignment = Element.ALIGN_LEFT;
            result.SpacingBefore = 10;
            result.SpacingAfter = 20;

            result.WidthPercentage = 100;
            result.SetWidths(new float[] { 18, 82 });
            result.AddCell(tit);
            result.AddCell(importe);

            return result;
        }

        public void AddLegales(Document document, CuentaFondoRTF listaFondos,List<string> legalesGenerales)
        {

            AddBr(document, 10);

            var legales = GetLegalesPorCuenta(listaFondos);

            foreach (var legal in legales)
            {
                var legalDescription = new Phrase(legal, GetFont(7, false, BusinessConstants.Colors.Font));
                document.Add(legalDescription);
                document.Add(Chunk.NEWLINE);
            }

            //var legalGeneral = GetLegalGeneral();

            document.Add(Chunk.NEWLINE);

            foreach (var legal in legalesGenerales)
            {
                var legalDescription = new Phrase(legal, GetFont(7, false, BusinessConstants.Colors.Font));
                document.Add(legalDescription);
                document.Add(Chunk.NEWLINE);
            }

            //foreach (var legal in legalGeneral)
            //{              
            //    var legalDescription = new Phrase(legal.Replace("|",""), GetFont(7, false, BusinessConstants.Colors.Font));
            //    document.Add(legalDescription);
            //    if(legal.Contains("|")) document.Add(Chunk.NEWLINE);
            //}

        }

        private List<string> GetLegalesFromFondos (CuentaFondoRTF listaFondos)
        {
            var legales = new List<string>();

            if (listaFondos.ListaEspeciesPesos != null)
            {
                foreach (var fondo in listaFondos.ListaEspeciesPesos)
                {

                    if (fondo.TieneLegales) legales.AddRange(fondo.ListaLegales);
                }

            }

            if (listaFondos.ListaEspeciesDolares != null)
            {
                foreach (var fondo in listaFondos.ListaEspeciesDolares)
                {

                    if (fondo.TieneLegales) legales.AddRange(fondo.ListaLegales);
                }

            }

            if (listaFondos.ListaEspeciesMovimientosDolares != null)
            {
                foreach (var fondo in listaFondos.ListaEspeciesMovimientosDolares)
                {

                    if (fondo.TieneLegales) legales.AddRange(fondo.ListaLegales);
                }

            }

            if (listaFondos.ListaEspeciesMovimientosPesos != null)
            {
                foreach (var fondo in listaFondos.ListaEspeciesMovimientosPesos)
                {

                    if (fondo.TieneLegales) legales.AddRange(fondo.ListaLegales);
                }

            }

            return legales.Count() > 0 ? legales.Distinct().ToList() : legales;
        }



        private List<string> GetLegalesPorCuenta(CuentaFondoRTF listaFondos)
        {
            var legales = new List<string>();

            if (listaFondos.Legales != null)
            {
                legales.AddRange(listaFondos.Legales);
            }

            
            return legales;
        }


        //private static List<Tuple<string, string>> GetLegales()
        //{
        //    var legales = new List<Tuple<string, string>>();
        //    var lines = File.ReadAllLines("./Images/legales.txt");
        //    foreach (var line in lines)
        //    {
        //        var parts = line.Split('|');
        //        legales.Add(new Tuple<string, string>(parts[0], parts[1]));
        //    }

        //    return legales;
        //}

        private static List<string> GetLegalGeneral()
        {
            var legales = new List<string>();
            var lines = File.ReadAllLines(HttpContext.Current.Server.MapPath("~/Images/Legales.txt"));
            foreach (var line in lines)
            {
                legales.Add(line);
            }

            return legales;
        }

        public void AddBr(Document doc, float size)
        {
            var spaceTable = new PdfPTable(1);
            spaceTable.WidthPercentage = 100;
            var spaceCell = new PdfPCell(new Phrase(""));
            spaceCell.FixedHeight = size;
            spaceCell.Border = Rectangle.NO_BORDER;

            spaceTable.AddCell(spaceCell);
            doc.Add(spaceTable);
        }

        //Titulo de la especie + tipo fondo.
        public PdfPTable MakeFCITitle(string value, string subtitulo)
        {
            var title = new PdfPTable(1);
            title.WidthPercentage = 60;
            var titPhrase = new Phrase();

            titPhrase.Add(new Chunk(value, GetFont(8, true, Colors.Font)));
            if (!string.IsNullOrEmpty(subtitulo))
                titPhrase.Add(new Chunk("\n\n" + subtitulo, GetFont(8, false, Colors.Font)));

            var cellTitle = new PdfPCell(titPhrase);

            cellTitle.BackgroundColor = GetBaseColour(Colors.BackgroundColorTableHeader);
            cellTitle.Border = Rectangle.NO_BORDER;
            cellTitle.HorizontalAlignment = Element.ALIGN_LEFT;
            title.AddCell(cellTitle);
            title.KeepTogether = true;
            return title;
        }

        //Tabla compuesta de nombre del fondo + tipo de fondo y resultado bruto. 
        public PdfPTable MakeFCIEspecieTitle(PdfPTable tituloEspecie, PdfPTable resBruto)
        {
            var result = new PdfPTable(2);
            result.WidthPercentage = 100;

            var leftCell = new PdfPCell(tituloEspecie);
            leftCell.Border = Rectangle.NO_BORDER;
            leftCell.HorizontalAlignment = Element.ALIGN_LEFT;
            leftCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            leftCell.Padding = 8;
            leftCell.BackgroundColor = GetBaseColour(Colors.BackgroundColorTableHeader);

            var rightCell = new PdfPCell(resBruto);
            rightCell.Border = Rectangle.NO_BORDER;
            rightCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rightCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            rightCell.Padding = 8;
            rightCell.BackgroundColor = GetBaseColour(Colors.BackgroundColorTableHeader);

            result.AddCell(leftCell);
            result.AddCell(rightCell);

            return result;
        }

        //Cuenta titulo y titulares en FCI
        public PdfPTable MakeCuentaTitulosTitle(Table owners, string cuentaTitulos)
        {
            var cuentaTituloCell = new PdfPCell(new Phrase($"Cuenta Títulos {cuentaTitulos}", GetFont(10, true, Colors.Font)));
            cuentaTituloCell.BackgroundColor = GetBaseColour(Colors.BackgroundColorTableHeader);
            cuentaTituloCell.Border = Rectangle.NO_BORDER;
            cuentaTituloCell.PaddingTop = 10;
            cuentaTituloCell.PaddingLeft = 8;

            var result = new PdfPTable(1);

            result.WidthPercentage = 100;
            result.SpacingBefore = 8;
            result.HorizontalAlignment = Rectangle.ALIGN_LEFT;

            //var titularesText = owners.Rows.First().Cells[1].GetFormatedValue();

            var bold = GetFont(9, true, Colors.Font);
            var regular = GetFont(9, false, Colors.Font);

            var titPhrase = new Paragraph();
            titPhrase.Add(new Chunk("Titulares: ", bold));
            titPhrase.Add(new Chunk("asdasd", regular));
            var titCell = new PdfPCell(titPhrase);

            titCell.PaddingLeft = 8;
            titCell.PaddingBottom = 10;
            titCell.BackgroundColor = GetBaseColour(Colors.BackgroundColorTableHeader);
            titCell.Border = Rectangle.NO_BORDER;

            result.AddCell(cuentaTituloCell);
            result.AddCell(titCell);

            result.KeepTogether = true;

            return result;
        }

        public PdfPTable MakeResultadoBrutoTableFCI(string resBrutoMoneda, string resBrutoPesos)
        {
            var font = GetFont(8, true, Colors.FontTableHeader);
            var fontTit = GetFont(8, false, Colors.FontTableHeader);
            var tbResBruto = new PdfPTable(2);
            tbResBruto.WidthPercentage = 100;
            tbResBruto.SetWidths(new float[] { 70, 30 });

            var titleMonedaCell = new PdfPCell(new Phrase("Inversor nro.", fontTit));
            titleMonedaCell.BackgroundColor = GetBaseColour(Colors.BackgroundColorTableHeader);
            titleMonedaCell.Border = Rectangle.NO_BORDER;
            titleMonedaCell.PaddingBottom = 6;
            titleMonedaCell.PaddingTop = 3;
            titleMonedaCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            titleMonedaCell.VerticalAlignment = Element.ALIGN_TOP;

            var importeMonedaCell = new PdfPCell(new Phrase(resBrutoMoneda, font));
            importeMonedaCell.BackgroundColor = GetBaseColour(Colors.BackgroundColorTableHeader);
            importeMonedaCell.Border = Rectangle.NO_BORDER;
            importeMonedaCell.PaddingRight = 14;
            importeMonedaCell.PaddingBottom = 6;
            importeMonedaCell.PaddingTop = 3;
            importeMonedaCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            importeMonedaCell.VerticalAlignment = Element.ALIGN_TOP;

            tbResBruto.AddCell(titleMonedaCell);
            tbResBruto.AddCell(importeMonedaCell);

            //if (!string.IsNullOrEmpty(resBrutoPesos))
            //{
            //    var titlePesosCell = new PdfPCell(new Phrase("Resultado bruto en pesos", font)); //Cambiar Font
            //    titlePesosCell.BackgroundColor = GetBaseColour(Colors.BackgroundColorTableHeader);
            //    titlePesosCell.Border = Rectangle.NO_BORDER;
            //    titlePesosCell.PaddingBottom = 6;
            //    titlePesosCell.PaddingTop = 3;
            //    titlePesosCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //    titlePesosCell.VerticalAlignment = Element.ALIGN_MIDDLE;

            //    var importePesosCell = new PdfPCell(new Phrase(resBrutoPesos, font));
            //    importePesosCell.BackgroundColor = GetBaseColour(Colors.BackgroundColorTableHeader);
            //    importePesosCell.Border = Rectangle.NO_BORDER;
            //    importePesosCell.PaddingRight = 8;
            //    importePesosCell.PaddingBottom = 6;
            //    importePesosCell.PaddingTop = 3;
            //    importePesosCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //    importePesosCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //    tbResBruto.AddCell(titlePesosCell);
            //    tbResBruto.AddCell(importePesosCell);
            //}

            return tbResBruto;
        }
    }
}
