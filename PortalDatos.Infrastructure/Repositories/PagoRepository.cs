using Dapper;
using Microsoft.Data.SqlClient;
using PortalDatos.Domain.Entities;
using PortalDatos.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalDatos.Infrastructure.Repositories
{
    public class PagoRepository : IPagoRepository
    {
        private readonly string _connectionString;

        public PagoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task GuardarPagosAsync(IEnumerable<PagoBoletaDigital> pagos)
        {
            // Cerrar y liberar la conexion automaticamente con using
            using var connection = new SqlConnection(_connectionString);

            var sql = @"INSERT INTO PagosBoletaDigital (NumeroBoleta, MontoCobrado, FechaCobro, OrigenArchivo)
            VALUES (@NumeroBoleta, @MontoCobrado, @FechaCobro, @OrigenArchivo)";

            await connection.ExecuteAsync(sql, pagos);
        }
    }
}
