using Ganss.Excel;
using PortalDatos.Domain.Entities;
using PortalDatos.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalDatos.Infrastructure.Processors
{
    public class ExcelProcessor : IFileProcessor
    {
        public string SupportedExtension => ".xlsx";

        public async Task<IEnumerable<PagoBoletaDigital>> ProcessFileAsync(string filePath)
        {
            // Todo dentro de un Task para mantener asincronica de la interfaz
            return await Task.Run(() =>
            {
                using var stream = File.OpenRead(filePath);
                var mapper = new ExcelMapper(stream);

                // ExcelMapper lee las columnas que coincidan con los nombres de las propiedades de la clase
                var pagos = mapper.Fetch<PagoBoletaDigital>().ToList();

                // Se les asigna el nombre del archivo del que vinieron
                pagos.ForEach(p => p.OrigenArchivo = Path.GetFileName(filePath));

                return pagos;
            });
        }
    }
}
