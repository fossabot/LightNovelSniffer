using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using LightNovelSniffer.Config;
using LightNovelSniffer.Exception;
using LightNovelSniffer.Web;
using Path = System.IO.Path;

namespace LightNovelSniffer.Output
{
    internal class PdfFooterEvent : IEventHandler
    {
        private readonly string title;

        public PdfFooterEvent(string chapterTitle)
        {
            title = chapterTitle;
        }

        public void HandleEvent(Event e)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent) e;
            PdfDocument pdf = docEvent.GetDocument();
            PdfPage page = docEvent.GetPage();
            Rectangle pageSize = page.GetPageSize();
            PdfCanvas pdfCanvas = new PdfCanvas(page.GetLastContentStream(), page.GetResources(), pdf);
            
            pdfCanvas.SaveState()
                .SetFillColor(new DeviceRgb(128, 128, 128))
                .Rectangle(pageSize.GetLeft(), pageSize.GetBottom(), pageSize.GetHeight(), pageSize.GetHeight())
                .Fill()
                .RestoreState();
            
            pdfCanvas.BeginText()
                .SetFontAndSize(PdfFontFactory.CreateFont(FontConstants.HELVETICA_OBLIQUE), 8)
                .MoveText(0, pageSize.GetBottom() - 20)
                .ShowText(title)
                .EndText();
            pdfCanvas.Release();
        }
    }

    public sealed class PdfFile : OutputFile
    {
        private PdfDocument pdf;
        private Document document;
        private PdfFooterEvent pdfFooterEvent;

        public PdfFile(LnParameters lnParam, string language)
        {
            InitiateDocument(lnParam, language);
        }
        
        protected override void InitiateDocument(LnParameters lnParam, string language)
        {
            this.lnParameters = lnParam;
            this.currentLanguage = language;

            pdf = TryGetPdf();
            document = new Document(pdf, PageSize.A4, false);
            if (!string.IsNullOrEmpty(lnParameters.urlCover))
                AddCover();

            foreach (string author in lnParam.authors)
                pdf.GetDocumentInfo().SetAuthor(author);
            pdf.GetDocumentInfo().AddCreationDate();
            pdf.GetDocumentInfo().SetMoreInfo("language", language);
            pdf.GetDocumentInfo().SetTitle(DocumentTitle);
            pdf.GetDocumentInfo().SetCreator(Globale.PUBLISHER);
        }

        private string GetPdfFileName()
        {
            return Path.Combine(OutputFolder, FileName + ".pdf");
        }

        private PdfDocument TryGetPdf()
        {
            PdfDocument doc = null;
            FileStream fs = new FileStream(GetPdfFileName(), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            try
            {
                doc = new PdfDocument(new PdfReader(fs), new PdfWriter(fs)) ;
            }
            catch (System.Exception)
            {
                doc = new PdfDocument(new PdfWriter(fs));
            }
            
            return doc;
        }

        private PdfOutline CreateOutline(PdfOutline outline, String title, Paragraph p)
        {
            if (outline == null)
            {
                outline = pdf.GetOutlines(false);
                outline = outline.AddOutline(title);
                return outline;
            }
            return outline;
        }

        public override void AddChapter(LnChapter lnChapter)
        {
            if (lnChapter.title == null)
            {
                lnChapter.title = string.Format(Globale.DEFAULT_CHAPTER_TITLE, lnChapter.chapNumber.ToString().PadLeft(3, '0'));
            }
            
            pdf.GetDocumentInfo().SetMoreInfo("LastChapter", lnChapter.chapNumber.ToString());

            PdfOutline outline = null;
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

            if (pdfFooterEvent != null)
                pdf.RemoveEventHandler(PdfDocumentEvent.END_PAGE, pdfFooterEvent);
            pdfFooterEvent = new PdfFooterEvent(lnChapter.title);
            pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, pdfFooterEvent);
            
            foreach (string paragraph in lnChapter.paragraphs.ParseHtmlNodeToStringList())
            {
                Paragraph p = new Paragraph(paragraph);
                p.SetTextAlignment(TextAlignment.JUSTIFIED_ALL);
                p.SetPaddingTop(15);
                if (outline == null)
                {
                    outline = CreateOutline(outline, lnChapter.title, p);
                }
                
                document.Add(p);
            }
        }
        
        public override void SaveDocument()
        {
            base.SaveDocument();
            document.Close();
        }

        private void AddCover()
        {
            try
            {
                byte[] image = WebCrawler.DownloadCover(lnParameters.urlCover);
                if (image != null && image.Length > 0)
                {
                    Image pic = new Image(ImageDataFactory.Create(image));
                    pic.SetAutoScale(true);
                    document.Add(pic);
                }
            } catch (CoverException)
            {}
        }
    }
}
