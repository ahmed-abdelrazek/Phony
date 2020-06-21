using Microsoft.Extensions.Logging;
using System;

namespace Phony.WPF
{
    public class ScopedService : IDisposable
    {
        private readonly ILogger _logger;

        public ScopedService(ILogger<ScopedService> logger)
        {
            _logger = logger;
            _logger.LogDebug("Instantiating ScopedService");
        }

        public void Dispose()
        {
            _logger.LogDebug("Disposing ScopedService");
        }
    }
}
