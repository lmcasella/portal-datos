using PortalDatos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalDatos.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObtenerPorUsernameAsync(string username);
        Task CrearUsuarioAsync(Usuario usuario);
    }
}
