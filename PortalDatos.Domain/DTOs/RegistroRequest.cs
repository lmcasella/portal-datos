using System;
using System.Collections.Generic;
using System.Text;

namespace PortalDatos.Domain.DTOs
{
    public class RegistroRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
