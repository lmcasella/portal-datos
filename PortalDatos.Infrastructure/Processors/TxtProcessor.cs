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

            // Leer archivo
            var lines = await File.ReadAllLinesAsync(filePath);

            if (lines.Length == 0)
            {
                throw new InvalidDataException($"El archivo {Path.GetFileName(filePath)} es invalido");
            }

            var listaPagos = new List<PagoBoletaDigital>();
            int numeroLinea = 1;

            // Txt separado por comas: NumeroBoleta,MontoCobrado,FechaCobro. Ignorar la primera linea (header)
            foreach (var line in lines.Skip(1))
            {
                numeroLinea++; // Sumamos 1 enseguida para que los errores marquen desde la línea 2 en adelante

                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(',');

                // Validar que sea de 3 columnas el archivo
                if (parts.Length != 3)
                {
                    throw new InvalidDataException($"Error de formato en la línea {numeroLinea}. Se esperaban 3 columnas pero hay {parts.Length}.");
                }

                if (!decimal.TryParse(parts[1], out var monto))
                    throw new InvalidCastException($"El monto '{parts[1]}' en la línea {numeroLinea} no es un número válido.");

                if (!DateTime.TryParse(parts[2], out var fecha))
                    throw new InvalidCastException($"La fecha '{parts[2]}' en la línea {numeroLinea} no tiene un formato válido.");

                listaPagos.Add(new PagoBoletaDigital
                {
                    NumeroBoleta = parts[0],
                    MontoCobrado = decimal.Parse(parts[1]),
                    FechaCobro = DateTime.Parse(parts[2]),
                    OrigenArchivo = Path.GetFileName(filePath)
                });
            }

            return listaPagos;
        }
    }
}
