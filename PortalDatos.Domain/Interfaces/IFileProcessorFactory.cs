using System;
using System.Collections.Generic;
using System.Text;

namespace PortalDatos.Domain.Interfaces
{
    public interface IFileProcessorFactory
    {
        IFileProcessor GetProcessor(string fileExtension);
    }
}
