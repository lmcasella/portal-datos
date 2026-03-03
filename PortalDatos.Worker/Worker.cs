using PortalDatos.Domain.Interfaces;

namespace PortalDatos.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IFileProcessorFactory _factory;

    // Directorios de trabajo (en un entorno real iria en el appsettings.json)
    private readonly string _inputFolder = @"C:\BoletasDigitales\Input";
    private readonly string _processedFolder = @"C:\BoletasDigitales\Processed";
    private readonly string _failedFolder = @"C:\BoletasDigitales\Failed";
    private readonly IPagoRepository _pagoRepository;

    // .NET inyecta el factory que ya tiene los procesadores adentro
    public Worker(ILogger<Worker> logger, IFileProcessorFactory factory, IPagoRepository pagoRepository)
    {
        _logger = logger;
        _factory = factory;
        _pagoRepository = pagoRepository;

        // Asegurar que las carpetas existen
        Directory.CreateDirectory(_inputFolder);
        Directory.CreateDirectory(_processedFolder);
        Directory.CreateDirectory(_failedFolder);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker de Boletas Digitales iniciado");

        while(!stoppingToken.IsCancellationRequested)
        {
            var files = Directory.GetFiles(_inputFolder);

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var extension = Path.GetExtension(file);

                try
                {
                    _logger.LogInformation($"Procesando archivo: {fileName}");

                    // Se le pide al factory la herramienta correcta para esta extension
                    var processor = _factory.GetProcessor(extension);

                    // Extraemos los datos
                    var boletas = await processor.ProcessFileAsync(file);

                    _logger.LogInformation($"Se extrajeron {boletas.Count()} pagos del archivo");

                    // Guardar boletas en SQL
                    if (boletas.Any())
                    {
                        _logger.LogInformation("Guardando pagos en la base de datos...");
                        await _pagoRepository.GuardarPagosAsync(boletas);
                        _logger.LogInformation("Guardado exitoso.");
                    }

                    // Si todo salio bien, mover archivo a Processed
                    var destPath = Path.Combine(_processedFolder, fileName);
                    File.Move(file, destPath, true);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error procesando el archivo {fileName}. Movimiendo a Failed.");

                    // Si algo falla, mover a Failed
                    var destPath = Path.Combine(_failedFolder, fileName);
                    File.Move(file, destPath, true);
                }
            }

            // Cada 5 segundos revisar la carpeta
            await Task.Delay(5000, stoppingToken);
        }
    }
}