// Helpers/PdfHelper.cs
using System;
using System.IO;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using EstacionamientoInteligente.Models;

namespace EstacionamientoInteligente.Helpers
{
    public static class PdfHelper
    {
        public static byte[] GenerarReportePdf(List<ReporteEstacionamiento> reporte, DateTime fecha)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4.Rotate(), 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                document.Open();

                // Agregar logo (asegúrate de tener una imagen logo.png en tu proyecto)
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "logo.png");
                Image logo = Image.GetInstance(imagePath);
                logo.ScaleToFit(100f, 100f);
                logo.Alignment = Element.ALIGN_LEFT;
                document.Add(logo);

                // Agregar título
                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 24, BaseColor.DARK_GRAY);
                Paragraph title = new Paragraph($"Reporte de Estacionamiento", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                Font dateFont = FontFactory.GetFont(FontFactory.HELVETICA, 14, BaseColor.GRAY);
                Paragraph dateParagraph = new Paragraph($"{fecha:dd MMMM yyyy}", dateFont);
                dateParagraph.Alignment = Element.ALIGN_CENTER;
                document.Add(dateParagraph);

                document.Add(Chunk.NEWLINE);

                // Crear tabla
                PdfPTable table = new PdfPTable(5);
                table.WidthPercentage = 100;
                float[] widths = new float[] { 2f, 3f, 3f, 2f, 2f };
                table.SetWidths(widths);

                // Estilos para la tabla
                Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.WHITE);
                Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                // Encabezados de la tabla
                string[] headers = { "Placa", "Hora de Entrada", "Hora de Salida", "Minutos", "Monto" };
                foreach (string header in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(header, headerFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.BackgroundColor = new BaseColor(52, 152, 219); // Azul
                    cell.Padding = 8;
                    table.AddCell(cell);
                }

                // Datos de la tabla
                foreach (var item in reporte)
                {
                    AddCell(table, item.Placa, cellFont);
                    AddCell(table, item.HoraEntrada.ToString("dd/MM/yyyy HH:mm:ss"), cellFont);
                    AddCell(table, item.HoraSalida.ToString("dd/MM/yyyy HH:mm:ss"), cellFont);
                    AddCell(table, item.MinutosEstacionado.ToString(), cellFont);
                    AddCell(table, item.MontoCobrado.ToString("C"), cellFont);
                }

                document.Add(table);

                // Agregar resumen
                document.Add(Chunk.NEWLINE);
                Font summaryFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, BaseColor.DARK_GRAY);
                Paragraph resumen = new Paragraph($"Total de vehículos: {reporte.Count}", summaryFont);
                document.Add(resumen);

                decimal totalIngreso = reporte.Sum(r => r.MontoCobrado);
                Paragraph ingresoTotal = new Paragraph($"Ingreso total: {totalIngreso:C}", summaryFont);
                document.Add(ingresoTotal);

                document.Close();
                writer.Close();

                return ms.ToArray();
            }
        }

        private static void AddCell(PdfPTable table, string text, Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 6;
            table.AddCell(cell);
        }
    }
}