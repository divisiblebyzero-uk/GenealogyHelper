using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GenealogyHelper.Service;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GenealogyHelper
{
    public class GenealogyOMatic : IHostedService
    {
        private GEDParser _gedParser;
        private ILogger<GenealogyOMatic> _logger;

        public GenealogyOMatic(ILogger<GenealogyOMatic> logger, GEDParser gedParser)
        {
            _gedParser = gedParser;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting application");
            _gedParser.ParseGEDFile($"d:\\dev\\projects\\genealogyhelper\\test.ged");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
