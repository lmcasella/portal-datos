using Dapper;
using Microsoft.Data.SqlClient;
using PortalDatos.Domain.DTOs;
using PortalDatos.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalDatos.Infrastructure.Repositories
{
    public class ReporteRepository : IReporteRepository
    {
        private readonly string _connectionString;

        public ReporteRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<ReportePagoDto>> ObtenerReportePagosAsync()
        {
            using var connection = new SqlConnection(_connectionString);

            var sql = @"
            SELECT 
                p.NumeroBoleta, 
                c.NombreCompleto, 
                p.MontoCobrado, 
                p.FechaCobro
            FROM PagosBoletaDigital p
            INNER JOIN Contribuyentes c ON p.NumeroBoleta = c.NumeroBoleta
            ORDER BY p.FechaCobro DESC";

            return await connection.QueryAsync<ReportePagoDto>(sql);
        }
    }
}
