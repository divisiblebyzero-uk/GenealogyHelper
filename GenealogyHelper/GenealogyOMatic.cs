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
        private readonly GEDLoader _gedLoader;
        private readonly ReportWriter _reportWriter;
        private readonly ILogger<GenealogyOMatic> _logger;

        public GenealogyOMatic(ILogger<GenealogyOMatic> logger, GEDLoader gedLoader, ReportWriter reportWriter)
        {
            _gedLoader = gedLoader;
            _reportWriter = reportWriter;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting application");
            _gedLoader.LoadGEDFile($"d:\\dev\\projects\\genealogyhelper\\test.ged");
            _reportWriter.WriteReport(@"d:\dev\projects\genealogyhelper\output.csv", _gedLoader.GEDModel);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
