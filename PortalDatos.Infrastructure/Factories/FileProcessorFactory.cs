using PortalDatos.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortalDatos.Infrastructure.Factories
{
    public class FileProcessorFactory : IFileProcessorFactory
    {
        private readonly IEnumerable<IFileProcessor> _processors;

        // Inyecta todos los procesadores que existan
        public FileProcessorFactory(IEnumerable<IFileProcessor> processors)
        {
            _processors = processors;
        }

        public IFileProcessor GetProcessor(string fileExtension)
        {
            // Buscar el procesador cuya extension coincida con la que se nos pide
            var processor = _processors.FirstOrDefault(p => p.SupportedExtension.Equals(fileExtension, StringComparison.OrdinalIgnoreCase));

            if (processor == null)
            {
                throw new NotSupportedException($"La extension {fileExtension} no es soportada");
            }

            return processor;
        }
    }
}
