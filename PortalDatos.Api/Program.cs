using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PortalDatos.Domain.Interfaces;
using PortalDatos.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
var jwtKey = builder.Configuration["Jwt:Key"]!;

builder.Services.AddTransient<IUsuarioRepository>(sp => new UsuarioRepository(connectionString));
builder.Services.AddTransient<IReporteRepository>(sp => new ReporteRepository(connectionString));

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontends", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173", // React
                "http://localhost:4200" // Angular
            )
            .AllowAnyHeader()  // Permite que React mande el Token JWT en el header
            .AllowAnyMethod(); // Permite GET, POST, PUT, DELETE, etc.
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors("PermitirFrontends");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();