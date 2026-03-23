using System;
using System.Collections.Generic;
using System.Text;

namespace PortalDatos.Domain.DTOs
{
    public class ReportePagoDto
    {
        public string NumeroBoleta { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public decimal MontoCobrado { get; set; }
        public DateTime FechaCobro { get; set; }
    }
}
