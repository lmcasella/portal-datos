using System;
using System.Collections.Generic;
using System.Text;

namespace PortalDatos.Domain.Entities
{
    public class PagoBoletaDigital
    {
        public int Id { get; set; }
        public string NumeroBoleta { get; set; } = string.Empty;
        public decimal MontoCobrado { get; set; }
        public DateTime FechaCobro { get; set; }
        public string OrigenArchivo { get; set; } = string.Empty; // De qué txt o Excel vino
    }
}
