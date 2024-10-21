using Isban.MapsMB.Common.Entity.Constantes;
using Isban.MapsMB.Common.Entity.Models;
using Isban.MapsMB.Common.Entity.Response;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Isban.MapsMB.Common.Entity.Constantes.BusinessConstants;
using static Isban.MapsMB.Common.Entity.Constantes.CoreConstants;

namespace Isban.MapsMB.FaxMailRTF
{
    public class RTFGeneracionPdF
    {
        public string CreacionPdf(CuentaFondoRTF listaFondos, string nup, string nombre,string pathParamter,string direccion,List<string> LegalesGenerales,string nombresClientes)
        {
            //pathParamter = @"\\bmgapp\apl\Pyxis\RTFTest";
            var dict = new Dictionary<string, string>();
            var filename = GetFilename(listaFondos.CtaTitulo.ToString(),nup,listaFondos.Periodo);
            string outputPath = Path.Combine(pathParamter, filename);

            var pdf = new PdfMaker();

            using (var doc = pdf.CreateDocument("A4"))
            {
                var writer = pdf.InitializeDocument(doc, outputPath, listaFondos.DescripcionCtaTitulo, listaFondos.Periodo, nombre, direccion,nombresClientes);

                AddFondosGroup(doc, listaFondos);

                //pdf.InsertNewPage(doc);
                pdf.AddLegales(doc, listaFondos, LegalesGenerales);

                pdf.CloseDocument(doc, writer);

                pdf.AddPageCount(doc, outputPath);

                //dict.Add(filename, outputPath);
            }

            return filename;
        }

        private string GetFilename(string cuenta, string nup,Periodo periodo)
        {
            return string.Format("RTF.{0}.{1}.{2}.{3}.pdf", cuenta, nup, periodo.FechaInicio.ToString("dd-MM-yyyy"), periodo.FechaFin.ToString("dd-MM-yyyy"));
        }

        public void AddFondosGroup(Document doc, CuentaFondoRTF listaFondos)
        {
            var pdf = new PdfMaker();

            GenerarTablasFondos(pdf,doc,listaFondos);

            GenerarTablasMovimientos(pdf, doc, listaFondos);
        }

        public void GenerarTablasFondos(PdfMaker Pdf, Document doc, CuentaFondoRTF listaFondos)
        {
            Pdf.AddBr(doc, 10);

            if (listaFondos.ListaEspeciesPesos != null && listaFondos.ListaEspeciesPesos.Count() > 0)
            {
                CrearTablaFondo(listaFondos.ListaEspeciesPesos, Pdf, doc,listaFondos.TotalPesos, string.Format("{0} {1}", TipoFondo.FondoPesos, listaFondos.Periodo.FechaFin.ToString("dd/MM/yyyy")), FormatType.CurrencyArs,FormatType.CuotaparteArs);

                Pdf.AddBr(doc, 14);
            }

            if (listaFondos.ListaEspeciesDolares != null && listaFondos.ListaEspeciesDolares.Count() > 0)
            {
                CrearTablaFondo(listaFondos.ListaEspeciesDolares, Pdf, doc, listaFondos.TotalDolares, string.Format("{0} {1}", TipoFondo.FondoDolares, listaFondos.Periodo.FechaFin.ToString("dd/MM/yyyy")), FormatType.CurrencyUsd,FormatType.CuotaparteUsd);

                Pdf.AddBr(doc, 14);
            }
        }

        public void CrearTablaFondo(List<EspecieFondoRTF> fondos, PdfMaker Pdf, Document doc, decimal? total, string tipoFondo, FormatType formatType, FormatType valorCuotaparte)
        {
            Font fontData = FontFactory.GetFont("OpenSans-Light", 8);
            Font fontDataBold = Pdf.GetFontStyle("OpenSans-SemiBold", 7, true, Colors.FontTableHeader);
            Font fontTotal = FontFactory.GetFont("OpenSans-Light", 8, Font.BOLD);

            Font fontDataBoldTit = Pdf.GetFontStyle("OpenSans-SemiBold", 8, true, Colors.FontTableHeader);
            Font fontDatos = Pdf.GetFontStyle("OpenSans-Light", 8, false, Colors.FontTableHeader);


            doc.AddTitle(tipoFondo, fontDataBoldTit, false, 0);

            Pdf.AddBr(doc, 8);

            var columns = new List<string> { DetalleFondos.Tipo, DetalleFondos.Fondo, DetalleFondos.Cuotapartes, DetalleFondos.ValorCuotaparte, DetalleFondos.TenenciaValuada };

            var table = new Table();
            table.ColumnCount = columns.Count();
            table.AddTitleRow(columns, fontDataBold);
            table.Rows[0].BackgroundColor = Colors.BackgroundColorTableHeader;
            table.Rows[0].HorizontalAlign = Alignment.LEFT;
            table.Rows[0].Cells[3].HorizontalAlign = Alignment.CENTER;
            table.Rows[0].Cells[4].HorizontalAlign = Alignment.RIGHT;
            table.Rows[0].Cells[4].Padding = 10;
            table.Rows[0].Cells[2].HorizontalAlign = Alignment.RIGHT;
            table.Rows[0].Cells[2].Padding = 5;


            var isFirstRow = true;

            foreach (var fondo in fondos)
            {
                var border = (!isFirstRow && !string.IsNullOrWhiteSpace(fondo.Tipo)) ? Border.TOP : Border.NO_BORDER;
                isFirstRow = false;

                List<Cell> cells = new List<Cell>{new Cell { Value = fondo.Tipo, Type = FormatType.String,HorizontalAlign= Alignment.LEFT ,BorderLine = border ,Font = fontDatos},
                    new Cell { Value = fondo.Fondo ,HorizontalAlign= Alignment.LEFT,BorderLine =border ,Font = fontDatos},
                    new Cell { Value = GetObjectValue(fondo.CantidadCuotapartes) ,Type = !string.IsNullOrEmpty(fondo.CantidadCuotapartes) ? FormatType.Cuotaparte : FormatType.String ,HorizontalAlign= Alignment.RIGHT,BorderLine =border ,Font = fontDatos,Padding = 5},
                    new Cell { Value = GetObjectValue(fondo.ValorCuotaparte),Type = !string.IsNullOrEmpty(fondo.ValorCuotaparte) ? valorCuotaparte : FormatType.String ,HorizontalAlign= Alignment.RIGHT,BorderLine =border,Font = fontDatos, Padding = 10},
                    new Cell { Value = GetObjectValue(fondo.TenenciaValuada),Type = fondo.TenenciaValuada != null ? formatType : FormatType.String ,HorizontalAlign= Alignment.RIGHT,BorderLine =border ,Font = fontDatos,Padding = 10}
                };

                var rowData2 = table.AddDataRow(cells, fontData);
            }

            //var cellEmpty = new Cell { Value = "Empty", hasColspan = true, Colspan = 3, isEmptyRow = true };
            //var rowEmpty = new Row();
            //rowEmpty.Cells.Add(cellEmpty);
            //table.Rows.Add(rowEmpty);


            //Footer table
            var cells2 = new List<Cell>{new Cell { Value ="Total" , Type = FormatType.String,HorizontalAlign= Alignment.LEFT  },
                    new Cell { Value = string.Empty ,HorizontalAlign= Alignment.LEFT },
                    new Cell { Value = string.Empty ,HorizontalAlign= Alignment.LEFT},
                    new Cell { Value = string.Empty ,HorizontalAlign= Alignment.LEFT },
                    new Cell { Value = GetObjectValue(total) , Type =  total != null ? formatType : FormatType.String, HorizontalAlign= Alignment.RIGHT,Padding = 10}

                };

        

            var rowFooter = new Row();
            rowFooter.Cells.AddRange(cells2);
            rowFooter.Type = CoreConstants.RowType.Footer;
            rowFooter.Font = fontDataBoldTit;
            rowFooter.BackgroundColor = Colors.BackgroundColorTableHeader;
            table.Rows.Add(rowFooter);

            var pdfTable = Pdf.MakeTable(table, 2);

            pdfTable.WidthPercentage = 100;
            pdfTable.HeaderRows = 1;
            Pdf.AddItem(doc, pdfTable);

            //if(table.Rows.Count() > 15) Pdf.InsertNewPage(doc); 

        }

        public void GenerarTablasMovimientos(PdfMaker Pdf, Document doc, CuentaFondoRTF listaFondos)
        {
            var contieneMovimientosPesos = listaFondos.ListaEspeciesMovimientosPesos != null && listaFondos.ListaEspeciesMovimientosPesos.Count() > 0;
            var contieneMovimientosDolares = listaFondos.ListaEspeciesMovimientosDolares != null && listaFondos.ListaEspeciesMovimientosDolares.Count() > 0;

            if (contieneMovimientosPesos || contieneMovimientosDolares)
            {
                if (listaFondos.ListaEspeciesPesos != null && listaFondos.ListaEspeciesPesos.Count() > 0 || listaFondos.ListaEspeciesDolares != null && listaFondos.ListaEspeciesDolares.Count() > 0)
                    Pdf.InsertNewPage(doc);
            }
            else
            {
                return;
            }

            //test
            if(listaFondos.CtaTitulo == 364795)
            {
                listaFondos.ListaEspeciesMovimientosPesos.FirstOrDefault().Total.CantidadCuotapartes = listaFondos.ListaEspeciesPesos.FirstOrDefault().CantidadCuotapartes;
                listaFondos.ListaEspeciesMovimientosPesos.FirstOrDefault().Total.ImporteNeto = listaFondos.ListaEspeciesPesos.FirstOrDefault().TenenciaValuada;

            }

            if (contieneMovimientosPesos)
            {
                CrearTablaMovimientos(listaFondos.ListaEspeciesMovimientosPesos, Pdf, doc, listaFondos.TotalPesos,
                    TipoFondo.MovimientosPesos, DetalleFondos.TotalFondosPesos, FormatType.CurrencyArs,FormatType.CuotaparteArs);

                Pdf.AddBr(doc, 14);
            }

            if (contieneMovimientosDolares)
            {
                CrearTablaMovimientos(listaFondos.ListaEspeciesMovimientosDolares, Pdf, doc, listaFondos.TotalDolares,
                    TipoFondo.MovimientosDolares, DetalleFondos.TotalFondosDolares, FormatType.CurrencyUsd,FormatType.CuotaparteUsd);

                Pdf.AddBr(doc, 14);
            }
        }

        public void CrearTablaMovimientos(List<MovimientoEspecieFondoRTF> fondoMovimientos, PdfMaker Pdf, Document doc, decimal? total, string tipoMovimiento, string totalLabel, FormatType currency,FormatType cuotaparteFormato)
        {

            Font fontData = FontFactory.GetFont("OpenSans-Light", 8);
            Font fontDataBold = FontFactory.GetFont("OpenSans-Light", 8 , Font.BOLD);
            var fontTitle = Pdf.GetFontStyle("OpenSans-SemiBold",7, true, Colors.FontTableHeader);
            var fontTotal = Pdf.GetFontStyle("OpenSans-SemiBold", 8, false, Colors.FontTableHeader);
            var fontTit = Pdf.GetFont(FontSize.TituloEspecie, true, Colors.Font);
            Font fontDatos = Pdf.GetFontStyle("OpenSans-Light", 8, false, Colors.FontTableHeader);
            var fontparaFondos = Pdf.GetFontStyle("OpenSans-SemiBold", 8, true, Colors.FontTableHeader);


            doc.AddTitle(tipoMovimiento, fontparaFondos, false, 0);
            Pdf.AddBr(doc, 8);
             var columns = new List<string> { DetalleFondos.Fecha, DetalleFondos.Concepto, DetalleFondos.Comprobante, DetalleFondos.Cuotapartes, DetalleFondos.ValorCuotaparte, DetalleFondos.Importe };


            var count = 0;
            foreach (var fondo in fondoMovimientos)
            {

                var lastFondo = fondoMovimientos.Last();

                var table = new Table();
                table.ColumnCount = columns.Count();
                table.AddTitleRow(columns, fontTitle);
                table.Rows[0].BackgroundColor = Colors.BackgroundColorTableHeader;
                table.Rows[0].HorizontalAlign = Alignment.LEFT;
                table.Rows[0].Cells[4].HorizontalAlign = Alignment.CENTER;
                table.Rows[0].Cells[5].HorizontalAlign = Alignment.RIGHT;
                table.Rows[0].Cells[3].HorizontalAlign = Alignment.RIGHT;
                table.Rows[0].Cells[3].Padding = 5;
                table.Rows[0].Cells[2].HorizontalAlign = Alignment.RIGHT;
                //table.Rows[0].Cells[2].Padding = 2;

                //table.Rows[0].Cells[4].Padding = 3;
                table.Rows[0].Cells[5].Padding = 5;


                var cellFondo = new Cell { Value = fondo.Fondo, HorizontalAlign = Alignment.LEFT, Bold = true, Font = fontparaFondos, hasColspan=true, Colspan = 6 , BorderLine = Border.BOTTOM };
                    var rowFondo = new Row();
                    rowFondo.Type = CoreConstants.RowType.Header;
                    rowFondo.Cells.Add(cellFondo);
                    table.Rows.Add(rowFondo);


                //if (count > 0)
                //{
                //    var cellFondo1 = new Cell { Value = fondo.Fondo, HorizontalAlign = Alignment.LEFT, Bold = true, Font = fontparaFondos, hasColspan = true, Colspan = 6, BorderLine = Border.BOTTOM };
                //    var rowFondo1 = new Row();
                //    rowFondo1.Cells.Add(cellFondo1);
                //    //rowFondo1.Type = CoreConstants.RowType.Header;

                //    table.Rows.Add(rowFondo1);
                //}

                if (fondo.SaldoAnterior != null)
                {

                    var cellsSaldoAnterior = new List<Cell>{new Cell { Value = fondo.SaldoAnterior.Fecha.ToString("dd/MM/yyyy"), Type = FormatType.String,HorizontalAlign= Alignment.LEFT ,Bold = true,Font = fontDatos },
                    new Cell { Value = "Saldo Anterior",HorizontalAlign= Alignment.LEFT ,Bold = true,Font = fontDatos},
                    new Cell { Value = string.Empty ,HorizontalAlign= Alignment.LEFT,Bold = true ,Font = fontDatos},
                    new Cell { Value = GetObjectValue(fondo.SaldoAnterior.CantidadCuotapartes),Type=  !string.IsNullOrEmpty(fondo.SaldoAnterior.CantidadCuotapartes) ? FormatType.Cuotaparte : FormatType.String ,HorizontalAlign= Alignment.RIGHT,Bold = true,Font = fontDatos,Padding = 5},
                    new Cell { Value = GetObjectValue(fondo.SaldoAnterior.ValorCuotaparte) , Type = !string.IsNullOrEmpty(fondo.SaldoAnterior.ValorCuotaparte) ? cuotaparteFormato : FormatType.String  ,HorizontalAlign= Alignment.RIGHT,Bold = true ,Font = fontDatos,Padding = 4},
                    new Cell { Value = GetObjectValue(fondo.SaldoAnterior.ImporteNeto), Type = fondo.SaldoAnterior.ImporteNeto != null ? currency : FormatType.String ,HorizontalAlign= Alignment.RIGHT,Bold = true ,Padding = 5,Font = fontDatos} };

                     table.AddDataRow(cellsSaldoAnterior, fontData);


                }



                    if (!fondo.ListaMovimientos.Any())
                    {
                        var cells = new List<Cell>{new Cell { Value = "Este fondo no registra movimientos durante este trimestre" , Type = FormatType.String,HorizontalAlign= Alignment.LEFT ,hasColspan =true, Colspan=4 ,Font = fontDatos,BorderLine = Border.BOTTOM},
                    new Cell { Value = string.Empty, Type = FormatType.String,HorizontalAlign= Alignment.CENTER,BorderLine = Border.BOTTOM},
                    new Cell { Value = string.Empty ,HorizontalAlign= Alignment.CENTER,BorderLine = Border.BOTTOM },
                    new Cell { Value = string.Empty ,HorizontalAlign= Alignment.CENTER ,BorderLine = Border.BOTTOM},
                    new Cell { Value = string.Empty ,HorizontalAlign= Alignment.CENTER,BorderLine = Border.BOTTOM },
                    new Cell { Value = string.Empty ,HorizontalAlign= Alignment.RIGHT , Padding = 5,BorderLine = Border.BOTTOM} };

                        table.AddDataRow(cells, fontData);
                    }
                    else
                    {
                        var last = fondo.ListaMovimientos.Last();

                        foreach (var movimiento in fondo.ListaMovimientos)
                        {

                            if (movimiento.Equals(last))
                            {
                                List<Cell> cells = new List<Cell>{new Cell { Value = movimiento.Fecha.ToString("dd/MM/yyyy"), Type = FormatType.String,HorizontalAlign= Alignment.LEFT  ,BorderLine =Border.BOTTOM,Font = fontDatos},
                    new Cell { Value = movimiento.Concepto ,HorizontalAlign= Alignment.LEFT,BorderLine =Border.BOTTOM,Font = fontDatos },
                    new Cell { Value = movimiento.Comprobante ,HorizontalAlign= Alignment.RIGHT,BorderLine =Border.BOTTOM ,Font = fontDatos},
                    new Cell { Value = GetObjectValue(movimiento.CantidadCuotapartes),Type= FormatType.Cuotaparte  ,HorizontalAlign= Alignment.RIGHT,BorderLine =Border.BOTTOM,Font = fontDatos,Padding = 5},
                    new Cell { Value = GetObjectValue(movimiento.ValorCuotaparte), Type = !string.IsNullOrEmpty(movimiento.ValorCuotaparte) ? cuotaparteFormato  : FormatType.String ,HorizontalAlign= Alignment.RIGHT ,BorderLine =Border.BOTTOM,Font = fontDatos,Padding = 4},
                    new Cell { Value = GetObjectValue(movimiento.ImporteNeto) , Type = movimiento.ImporteNeto != null ? currency : FormatType.String ,HorizontalAlign= Alignment.RIGHT,BorderLine =Border.BOTTOM ,Padding = 5,Font = fontDatos} };

                                table.AddDataRow(cells, fontData);
                            }
                            else
                            {

                                List<Cell> cells = new List<Cell>{new Cell { Value = movimiento.Fecha.ToString("dd/MM/yyyy"), Type = FormatType.String,HorizontalAlign= Alignment.LEFT,Font = fontDatos  },
                    new Cell { Value = movimiento.Concepto ,HorizontalAlign= Alignment.LEFT ,Font = fontDatos},
                    new Cell { Value = movimiento.Comprobante ,HorizontalAlign= Alignment.RIGHT ,Font = fontDatos},
                    new Cell { Value = GetObjectValue(movimiento.CantidadCuotapartes),Type= FormatType.Cuotaparte  ,HorizontalAlign= Alignment.RIGHT,Font = fontDatos,Padding = 5},
                    new Cell { Value =  GetObjectValue(movimiento.ValorCuotaparte), Type = !string.IsNullOrEmpty(movimiento.ValorCuotaparte) ? cuotaparteFormato : FormatType.String ,HorizontalAlign= Alignment.RIGHT,Font = fontDatos,Padding = 4 },
                    new Cell { Value =  GetObjectValue(movimiento.ImporteNeto) , Type = movimiento.ImporteNeto != null ? currency : FormatType.String ,HorizontalAlign= Alignment.RIGHT ,Padding = 5,Font = fontDatos} };

                                table.AddDataRow(cells, fontData);
                            }

                        }
                    }

                if (fondo.Total != null)
                {


                    var cellsTenencia = new List<Cell>{new Cell { Value = fondo.Total.Fecha.ToString("dd/MM/yyyy"), Type = FormatType.String,HorizontalAlign= Alignment.LEFT ,BorderLine =Border.BOTTOM,Bold = true },
                    new Cell { Value = "Tenencia Actual",HorizontalAlign= Alignment.LEFT,BorderLine =Border.BOTTOM ,Bold = true},
                    new Cell { Value = string.Empty ,HorizontalAlign= Alignment.LEFT,BorderLine =Border.BOTTOM,Bold = true },
                    new Cell { Value = GetObjectValue(fondo.Total.CantidadCuotapartes) ,Type= FormatType.Cuotaparte   ,HorizontalAlign= Alignment.RIGHT,BorderLine =Border.BOTTOM,Bold = true,Padding = 5},
                     new Cell { Value = GetObjectValue(fondo.Total.ValorCuotaparte) ,  Type = !string.IsNullOrEmpty(fondo.Total.ValorCuotaparte)  ? cuotaparteFormato : FormatType.String ,HorizontalAlign= Alignment.RIGHT,BorderLine =Border.BOTTOM,Bold = true,Padding = 4},
                    new Cell { Value = GetObjectValue(fondo.Total.ImporteNeto),  Type = fondo.Total.ImporteNeto != null ? currency : FormatType.String ,HorizontalAlign= Alignment.RIGHT,BorderLine =Border.BOTTOM,Bold = true  ,Padding = 5,Font = fontDataBold} };

                    table.AddDataRow(cellsTenencia, fontDataBold);
                }


               

                if (fondo.Equals(lastFondo))
                {

                    var cellEmpty = new Cell { Value = "Empty", hasColspan = true, Colspan = 6, isEmptyRow = true };
                    var rowEmpty = new Row();
                    rowEmpty.Cells.Add(cellEmpty);
                    table.Rows.Add(rowEmpty);

                    var cells2 = new List<Cell>{new Cell { Value = totalLabel , Type = FormatType.String,HorizontalAlign= Alignment.LEFT ,hasColspan =true, Colspan=2 },
                    new Cell { Value = string.Empty ,HorizontalAlign= Alignment.CENTER},
                    new Cell { Value = string.Empty ,HorizontalAlign= Alignment.CENTER },
                    new Cell { Value = string.Empty ,HorizontalAlign= Alignment.CENTER },
                    new Cell { Value = string.Empty ,HorizontalAlign= Alignment.CENTER },
                    new Cell { Value = GetObjectValue(total), Type =total == null ? FormatType.String : currency ,HorizontalAlign= Alignment.RIGHT ,Padding = 5}

                     };


                    var rowFooter = new Row();
                    rowFooter.Font = fontparaFondos;
                    rowFooter.Cells.AddRange(cells2);
                    rowFooter.Type = CoreConstants.RowType.Footer;
                    rowFooter.BackgroundColor = Colors.BackgroundColorTableHeader;
                    table.Rows.Add(rowFooter);
                }


                var pdfTable = Pdf.MakeTable(table, null);

                pdfTable.WidthPercentage = 100;
                pdfTable.HeaderRows = 2;
                //pdfTable.KeepTogether = true;

                //if (count > 0)
                //pdfTable.SkipFirstHeader = true;
                //pdfTable.SplitLate = true;
                //pdfTable.SplitRows = false;
                Pdf.AddItem(doc, pdfTable);

                Pdf.AddBr(doc, 22);

                count++;

            }
            
        }


        private Object GetObjectValue(string objeto)
        {
            var objectValue = new Object();
            if (string.IsNullOrEmpty(objeto))
            {
                objectValue = "-";
            }
            else
            {
                objectValue = double.Parse(objeto);
            }

            return objectValue;
        }

        private Object GetObjectValue(decimal? objeto)
        {
            var objectValue = new Object();
            if (objeto == null)
            {
                objectValue = "-";
            }
            else
            {
                objectValue = objeto;
            }

            return objectValue;
        }



    }
  
    public static class ExtensionesDoc
    {
        private static PdfMaker pdf = new PdfMaker();

        public static void AddTable(this iTextSharp.text.Document doc, Table table, float? widthPercentage = null, int? spacing = null)
        {
            var pdfTable = pdf.MakeTable(table, spacing);
            if (widthPercentage != null) pdfTable.WidthPercentage = widthPercentage.Value;
            doc.Add(pdfTable);
        }

        public static void AddTitle(this iTextSharp.text.Document doc, string text, Font font, bool centered, int? spacing)
        {
            doc.Add(pdf.MakeTitle(text, font, centered, spacing));
        }

        public static void AddTitleWithImage(this iTextSharp.text.Document doc, string text, string subText, Font font, Font subTextFont, int? spacingBefore)
        {
            doc.Add(pdf.MakeGroupTitle(text, subText, font, subTextFont, spacingBefore));
        }
    }
}
