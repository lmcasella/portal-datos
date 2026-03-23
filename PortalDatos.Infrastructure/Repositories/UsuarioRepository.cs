using Microsoft.Data.SqlClient;
using PortalDatos.Domain.Entities;
using PortalDatos.Domain.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalDatos.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Usuario?> ObtenerPorUsernameAsync(string username)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT Id, Username, PasswordHash FROM Usuarios WHERE Username = @Username";
            return await connection.QuerySingleOrDefaultAsync<Usuario>(sql, new { Username = username });
        }

        public async Task CrearUsuarioAsync(Usuario usuario)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "INSERT INTO Usuarios (Username, PasswordHash) VALUES (@Username, @PasswordHash)";
            await connection.ExecuteAsync(sql, usuario);
        }
    }
}
