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
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                document.Open();

                // Agregar título
                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                Paragraph title = new Paragraph($"Reporte de Estacionamiento - {fecha:dd/MM/yyyy}", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                document.Add(Chunk.NEWLINE);

                // Crear tabla
                PdfPTable table = new PdfPTable(5);
                table.WidthPercentage = 100;

                // Encabezados de la tabla
                string[] headers = { "Placa", "Hora de Entrada", "Hora de Salida", "Minutos", "Monto" };
                foreach (string header in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(header, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);
                }

                // Datos de la tabla
                foreach (var item in reporte)
                {
                    table.AddCell(item.Placa);
                    table.AddCell(item.HoraEntrada.ToString("dd/MM/yyyy HH:mm:ss"));
                    table.AddCell(item.HoraSalida.ToString("dd/MM/yyyy HH:mm:ss"));
                    table.AddCell(item.MinutosEstacionado.ToString());
                    table.AddCell(item.MontoCobrado.ToString("C"));
                }

                document.Add(table);

                // Agregar resumen
                document.Add(Chunk.NEWLINE);
                Paragraph resumen = new Paragraph($"Total de vehículos: {reporte.Count}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12));
                document.Add(resumen);

                decimal totalIngreso = reporte.Sum(r => r.MontoCobrado);
                Paragraph ingresoTotal = new Paragraph($"Ingreso total: {totalIngreso:C}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12));
                document.Add(ingresoTotal);

                document.Close();
                writer.Close();

                return ms.ToArray();
            }
        }
    }
}