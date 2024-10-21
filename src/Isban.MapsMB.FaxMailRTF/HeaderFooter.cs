using Isban.MapsMB.Common.Entity.Models;
using Isban.MapsMB.Common.Entity.Response;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static Isban.MapsMB.Common.Entity.Constantes.BusinessConstants;
using static Isban.MapsMB.Common.Entity.Constantes.CoreConstants;

namespace Isban.MapsMB.FaxMailRTF
{
    public class HeaderFooter : PdfPageEventHelper
    {
        string numInveror = string.Empty;
        string nombreCliente = string.Empty;
        string direccion = string.Empty;
        string nombresClientes = string.Empty;
        Periodo periodo = new Periodo();

        public HeaderFooter(string numInveror, string nombreCliente, string direccion, Periodo periodo,string nombresClientes)
        {
            this.numInveror = numInveror;
            this.nombreCliente = nombreCliente;
            this.nombresClientes = nombresClientes;
            this.direccion = direccion;
            this.periodo = periodo;
        }

        static Image logoSocDepositaria = Image.GetInstance(HttpContext.Current.Server.MapPath("~/Images/LogoSocDepositaria.png"));
        static Image logoSocGerente = Image.GetInstance(HttpContext.Current.Server.MapPath("~/Images/LogoSocGerente.png"));

        static Font fontDefault = FontFactory.GetFont("open sans", 8, Font.NORMAL, BaseColor.BLACK);
        static Font fontLegales = FontFactory.GetFont("OpenSans-Light", 6);

        public override void OnStartPage(PdfWriter writer, Document document)
        {
            var pdf = new PdfMaker();

            pdf.AddBr(document, 3);

            base.OnStartPage(writer, document);
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {

            //Header
            var tbHeader = new PdfPTable(2);
            tbHeader.TotalWidth = document.PageSize.Width - document.LeftMargin;
            var cell = new PdfPCell(new Phrase(Segmento.RTF, FontFactory.GetFont("OpenSans-Light", 8, BaseColor.RED)));
            cell.Border = 0;
            cell.PaddingLeft = 35;
            tbHeader.AddCell(cell);

            var cellPeriodo = new PdfPCell(new Phrase(string.Format("Período: {0} - {1}", periodo.FechaInicio.ToString("dd/MM/yyyy"), periodo.FechaFin.ToString("dd/MM/yyyy")), FontFactory.GetFont("OpenSans-Light", 8, BaseColor.BLACK)));
            cellPeriodo.HorizontalAlignment = Element.ALIGN_RIGHT;
            cellPeriodo.Border = 0;
            cellPeriodo.PaddingRight = 0;
            tbHeader.AddCell(cellPeriodo);
            tbHeader.WriteSelectedRows(0, -1, 0, 820, writer.DirectContent);

            if (document.PageNumber > 1)
            {
                var cliente = MakeFCITitle(string.Format("Inversor nro. {0}", numInveror), nombresClientes, 10,6);
                var table = MakeFCIEspecieTitle(cliente, MakeNumInversor(string.Empty, string.Empty));
                table.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                table.WriteSelectedRows(0, -1, document.LeftMargin, 800, writer.DirectContent);
            }
            else
            {
                var cliente = MakeFCITitle(nombreCliente, direccion,8,8);
                var table = MakeFCIEspecieTitle(cliente, MakeNumInversor("Inversor nro.", numInveror));
                table.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                table.WriteSelectedRows(0, -1, document.LeftMargin, 800, writer.DirectContent);
            }


            //Footer
            var tbFooter = new PdfPTable(2);
            tbFooter.TotalWidth = document.PageSize.Width;

            var cellLogo = new PdfPCell(logoSocDepositaria);
            cellLogo.HorizontalAlignment = Element.ALIGN_LEFT;
            cellLogo.FixedHeight = 33;
            cellLogo.PaddingBottom = 0;
            cellLogo.PaddingLeft = 30;
            cellLogo.Border = 0;
            cellLogo.VerticalAlignment = Element.ALIGN_CENTER;
            tbFooter.AddCell(cellLogo);

            var cellLogoGerente = new PdfPCell(logoSocGerente);
            cellLogoGerente.HorizontalAlignment = Element.ALIGN_RIGHT;
            cellLogoGerente.FixedHeight = 40;
            cellLogoGerente.PaddingBottom = 0;
            cellLogoGerente.PaddingRight = 30;
            cellLogoGerente.Border = 0;
            cellLogoGerente.VerticalAlignment = Element.ALIGN_CENTER;
            tbFooter.AddCell(cellLogoGerente);
            tbFooter.WriteSelectedRows(0, -1, 0, 65, writer.DirectContent);

            //Footer Legales
            //var tbFooterLegal = new PdfPTable(1);
            //tbFooterLegal.TotalWidth = document.PageSize.Width;

            //var legales = new PdfPCell(new Phrase("(*) En los fondos en especies las cuotapartes representan la cantidad nominal de Letras que te corresponden en función a tu participación en el fondo.___ SEGÚN R.G. 481 / 05 DE LA CNV EL MARGEN DE LIQUIDEZ DEL SUPER AHORRO $ SERA DEPOSITADO EN CUENTAS EN EL BCRA.ESTE FONDO INCORPORO A SU CARTERA DE INVERSION LETRAS DEL BCRA. DE ACUERDO A LA RES GRAL 481 / 05 DEL 17 / 08 / 05 DE LA CNV EL MARGEN DE LIQUIDEZ DEL SUPER AHORRO $ SERA DEPOSITADO EN CUENTAS EN EL BCRA.INFORMAMOS ADEMAS QUE ESTE FONDO INCORPORO A SU CARTERA DE INVERSION LEBAC(LETRAS DE BCRA).", fontLegales));
            //legales.Border = 0;
            //legales.PaddingLeft = 30;
            //legales.PaddingRight = 30;            
            //legales.HorizontalAlignment = Element.ALIGN_CENTER;

            //tbFooterLegal.AddCell(legales);
            //tbFooterLegal.WriteSelectedRows(0, -1, 0, tbFooter.TotalHeight + 60, writer.DirectContent);

        }
        public PdfPTable MakeFCITitle(string value, string direccion, int sizeValue1 ,int sizeValue2)
        {
            var title = new PdfPTable(1);
            title.WidthPercentage = 60;
            var titPhrase = new Phrase();

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            if (!string.IsNullOrEmpty(value)) value = textInfo.ToTitleCase(textInfo.ToLower(value));

            //if (!string.IsNullOrEmpty(direccion)) direccion = textInfo.ToTitleCase(textInfo.ToLower(direccion));

            titPhrase.Add(new Chunk(value, GetFont(sizeValue1, true, Colors.Font)));
            //if (!string.IsNullOrEmpty(subtitulo))
            titPhrase.Add(new Chunk("\n\n" + direccion, GetFont(sizeValue2, false, Colors.Font)));
            if (string.IsNullOrEmpty(direccion))
                titPhrase.Add(new Chunk("\n\n", GetFont(sizeValue2, false, Colors.Font)));


            var cellTitle = new PdfPCell(titPhrase);

            cellTitle.BackgroundColor = GetBaseColour(Colors.BackgroundColorTableHeader);
            cellTitle.Border = Rectangle.NO_BORDER;
            cellTitle.HorizontalAlignment = Element.ALIGN_LEFT;
            title.AddCell(cellTitle);
            title.KeepTogether = true;
            return title;
        }

        public PdfPTable MakeNumInversor(string value, string subtitulo)
        {
            var title = new PdfPTable(1);
            title.WidthPercentage = 100;
            var titPhrase = new Phrase();

            titPhrase.Add(new Chunk(value, GetFont(8, false, Colors.Font)));
            titPhrase.Add(new Chunk(subtitulo, GetFont(8, true, Colors.Font)));

            var cellTitle = new PdfPCell(titPhrase);

            cellTitle.BackgroundColor = GetBaseColour(Colors.BackgroundColorTableHeader);
            cellTitle.Border = Rectangle.NO_BORDER;
            cellTitle.HorizontalAlignment = Element.ALIGN_RIGHT;
            cellTitle.VerticalAlignment = Element.ALIGN_TOP;
            title.AddCell(cellTitle);
            title.KeepTogether = true;
            return title;
        }
        public PdfPTable MakeNumInversorHeader(string numInversor)
        {
            var font = GetFont(6, true, Colors.FontTableHeader);
            var fontTit = GetFont(8, false, Colors.FontTableHeader);
            var tbResBruto = new PdfPTable(2);
            tbResBruto.WidthPercentage = 100;
            tbResBruto.SetWidths(new float[] { 70, 30 });

            var titleMonedaCell = new PdfPCell(new Phrase("Inversor nro.", fontTit));
            var phase = new Phrase(numInversor, font);
            titleMonedaCell.AddElement(phase);
            titleMonedaCell.BackgroundColor = GetBaseColour(Colors.BackgroundColorTableHeader);
            titleMonedaCell.Border = Rectangle.NO_BORDER;
            titleMonedaCell.PaddingBottom = 6;
            titleMonedaCell.PaddingTop = 1;
            titleMonedaCell.PaddingRight = 20;
            titleMonedaCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            titleMonedaCell.VerticalAlignment = Element.ALIGN_TOP;

            //var importeMonedaCell = new PdfPCell(new Phrase(numInversor, font));
            //importeMonedaCell.BackgroundColor = GetBaseColour(Colors.BackgroundColorTableHeader);
            //importeMonedaCell.Border = Rectangle.NO_BORDER;
            //importeMonedaCell.PaddingRight = 10;
            //importeMonedaCell.PaddingBottom = 6;
            //importeMonedaCell.PaddingTop = 1;
            //importeMonedaCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //importeMonedaCell.VerticalAlignment = Element.ALIGN_TOP;

            tbResBruto.AddCell(titleMonedaCell);
            //tbResBruto.AddCell(importeMonedaCell);

            return tbResBruto;
        }

        public PdfPTable MakeFCIEspecieTitle(PdfPTable cliente, PdfPTable numInveror)
        {
            var result = new PdfPTable(2);
            result.WidthPercentage = 100;

            var leftCell = new PdfPCell(cliente);
            leftCell.Border = Rectangle.NO_BORDER;
            leftCell.HorizontalAlignment = Element.ALIGN_LEFT;
            leftCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            leftCell.Padding = 8;
            leftCell.BackgroundColor = GetBaseColour(Colors.BackgroundColorTableHeader);

            var rightCell = new PdfPCell(numInveror);
            rightCell.Border = Rectangle.NO_BORDER;
            rightCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rightCell.VerticalAlignment = Element.ALIGN_TOP;
            rightCell.Padding = 8;
            rightCell.BackgroundColor = GetBaseColour(Colors.BackgroundColorTableHeader);

            result.AddCell(leftCell);
            result.AddCell(rightCell);

            return result;
        }
        public Font GetFont(float size, bool bold, string colour)
        {
            var fontStyle = bold ? Font.BOLD : Font.NORMAL;
            var fontColour = GetBaseColour(colour);
            var font = new Font(FontFactory.GetFont("open sans", size, fontStyle, fontColour));
            return font;
        }
        private BaseColor GetBaseColour(string code)
        {
            var color = System.Drawing.ColorTranslator.FromHtml(code);
            return new BaseColor(color);
        }
    }
}
