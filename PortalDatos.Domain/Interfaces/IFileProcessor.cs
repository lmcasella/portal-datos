using PortalDatos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalDatos.Domain.Interfaces
{
    public interface IFileProcessor
    {
        // Esta propiedad sirve para identificar que procesador usar (.txt o .xlsx)
        string SupportedExtension { get; }

        Task<IEnumerable<PagoBoletaDigital>> ProcessFileAsync(string filePath);
    }
}
