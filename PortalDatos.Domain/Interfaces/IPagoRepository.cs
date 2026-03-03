using PortalDatos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalDatos.Domain.Interfaces
{
    public interface IPagoRepository
    {
        Task GuardarPagosAsync(IEnumerable<PagoBoletaDigital> pagos);
    }
}
