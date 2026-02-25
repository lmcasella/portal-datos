using PortalDatos.Domain.Entities;
using PortalDatos.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalDatos.Infrastructure.Processors
{
    public class TxtProcessor : IFileProcessor
    {
        // La extension que maneja esta clase
        public string SupportedExtension => ".txt";

        public async Task<IEnumerable<PagoBoletaDigital>> ProcessFileAsync(string filePath)
        {
            var listaPagos = new List<PagoBoletaDigital>();

            // Leer archivo
            var lines = await File.ReadAllLinesAsync(filePath);

            // Txt separado por comas: NumeroBoleta,MontoCobrado,FechaCobro.
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 3)
                {
                    listaPagos.Add(new PagoBoletaDigital
                    {
                        NumeroBoleta = parts[0],
                        MontoCobrado = decimal.Parse(parts[1]),
                        FechaCobro = DateTime.Parse(parts[2]),
                        OrigenArchivo = Path.GetFileName(filePath)
                    });
                }
            }

            return listaPagos;
        }
    }
}
