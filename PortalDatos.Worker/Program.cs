using PortalDatos.Domain.Interfaces;
using PortalDatos.Infrastructure.Factories;
using PortalDatos.Infrastructure.Processors;
using PortalDatos.Infrastructure.Repositories;
using PortalDatos.Worker;

var builder = Host.CreateApplicationBuilder(args);

string connectionString = "Data Source=.\\LCASELLA;Initial Catalog=Test;Integrated Security=True;TrustServerCertificate=True;";
builder.Services.AddTransient<IPagoRepository>(sp => new PagoRepository(connectionString));

// Registrar Factory
builder.Services.AddSingleton<IFileProcessorFactory, FileProcessorFactory>();

// Registrar Procesadores
builder.Services.AddSingleton<IFileProcessor, TxtProcessor>();
builder.Services.AddSingleton<IFileProcessor, ExcelProcessor>();

// Registrar Worker
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
