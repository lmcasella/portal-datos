//using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PortalDatos.Domain.Interfaces;
using PortalDatos.Domain.Entities;
using BCrypt.Net;
using PortalDatos.Domain.DTOs;

namespace PortalDatos.Api.Controllers
{
    public class AuthRequestDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IConfiguration _config;

        public AuthController(IUsuarioRepository usuarioRepo, IConfiguration config)
        {
            _usuarioRepo = usuarioRepo;
            _config = config;
        }

        // Endpoint para crear un usuario de prueba encriptado
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegistroRequest request)
        {
            var usuarioExistente = await _usuarioRepo.ObtenerPorUsernameAsync(request.Username);
            if (usuarioExistente != null)
            {
                return BadRequest(new { mensaje = "El nombre de usuario ya está en uso." });
            }

            var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            await _usuarioRepo.CrearUsuarioAsync(new Usuario { Username = request.Username, PasswordHash = hash });
            return Ok("Usuario creado.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequestDto request)
        {
            var usuario = await _usuarioRepo.ObtenerPorUsernameAsync(request.Username);

            // Verificar si existe y si la pass coincide con el Hash de la base
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
            {
                return Unauthorized("Credenciales incorrectas");
            }

            // Generar Token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, usuario.Username) }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new { Token = tokenHandler.WriteToken(token) });
        }
    }
}
