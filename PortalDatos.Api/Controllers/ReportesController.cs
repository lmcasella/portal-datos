using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalDatos.Domain.Interfaces;

namespace PortalDatos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Esto exige un token JWT valido para entrar
    public class ReportesController : ControllerBase
    {
        private readonly IReporteRepository _reporteRepo;

        public ReportesController(IReporteRepository reporteRepo)
        {
            _reporteRepo = reporteRepo;
        }

        [HttpGet("pagos")]
        public async Task<IActionResult> ObtenerReportePagos()
        {
            // Consumir el Repositorio con el JOIN
            var reporte = await _reporteRepo.ObtenerReportePagosAsync();
            return Ok(reporte);
        }
    }
}
