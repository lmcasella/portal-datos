using PortalDatos.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalDatos.Domain.Interfaces
{
    public interface IReporteRepository
    {
        Task<IEnumerable<ReportePagoDto>> ObtenerReportePagosAsync();
    }
}
