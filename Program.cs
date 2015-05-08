using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using iTextSharp.text;

namespace PDFWaterMark
{
    public class program
    {
        public static void Main()
        {
            string inputPath = @"C:\FileRecv\V01.pdf";
            string outputPath = @"V01.pdf";
            string watermarkPath = @"‪a.jpg";
            Watermark2(inputPath,outputPath,watermarkPath);
        }

        public static void Watermark(string inputPath, string outputPath, string watermarkPath)
        {

            try
            {

                PdfReader reader = new PdfReader(inputPath);

                iTextSharp.text.Document document = new iTextSharp.text.Document();
                FileStream fs = new FileStream(outputPath, FileMode.OpenOrCreate);
                PdfWriter writer = PdfWriter.GetInstance(document,fs);
                document.Open();
                var imagewatermark = iTextSharp.text.Image.GetInstance(watermarkPath);
                document.Add(imagewatermark);
                document.Open();
                PdfContentByte cb = writer.DirectContent;
                PdfImportedPage newPage;
                int iPageNum = reader.NumberOfPages;
                for (int j = 1; j <= iPageNum; j++)
                {
                    document.NewPage();
                    newPage = writer.GetImportedPage(reader, j);
                    cb.AddTemplate(newPage, 0, 0);
                }
                document.Close();
                writer.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                // WriteLog.Log(ex.ToString());
                throw ex;
            }
        }

        public static void Watermark2(string inputPath, string outputPath, string watermarkPath)
        {
            //create pdfreader object to read sorce pdf
            PdfReader pdfReader = new PdfReader(inputPath);
            //create stream of filestream or memorystream etc. to create output file
            FileStream stream = new FileStream(outputPath, FileMode.OpenOrCreate);
            //create pdfstamper object which is used to add addtional content to source pdf file
            PdfStamper pdfStamper = new PdfStamper(pdfReader, stream);
            //iterate through all pages in source pdf
            for (int pageIndex = 1; pageIndex <= pdfReader.NumberOfPages; pageIndex++)
            {
                //Rectangle class in iText represent geomatric representation... in this case, rectanle object would contain page geomatry
                var pageRectangle = pdfReader.GetPageSizeWithRotation(pageIndex);
                //pdfcontentbyte object contains graphics and text content of page returned by pdfstamper
                PdfContentByte pdfData = pdfStamper.GetOverContent(pageIndex);
                //Add image 
                var imagewatermark = iTextSharp.text.Image.GetInstance(watermarkPath);
                imagewatermark.SetAbsolutePosition(0,pageRectangle.Height/2);
                pdfData.AddImage(imagewatermark);
                ////create fontsize for watermark
                //pdfData.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 40);
                ////create new graphics state and assign opacity
                //PdfGState graphicsState = new PdfGState();
                //graphicsState.FillOpacity = 0.4F;
                ////set graphics state to pdfcontentbyte
                //pdfData.SetGState(graphicsState);
                ////set color of watermark
                //pdfData.SetColorFill(BaseColor.RED);
                ////indicates start of writing of text
                //pdfData.BeginText();
                ////show text as per position and rotation
                //pdfData.ShowTextAligned(Element.ALIGN_CENTER, "BlueLemonCodeajdksakjdaskjdakjdksajdksa", pageRectangle.Width / 2, pageRectangle.Height / 2, 45);
                ////call endText to invalid font set
                //pdfData.EndText();
            }
            //close stamper and output filestream
            pdfStamper.Close();
            stream.Close();
        }
    }
}
