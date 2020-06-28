using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GenealogyHelper.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Settings.Configuration;

namespace GenealogyHelper
{
    public class GenealogyOMatic : IHostedService
    {
        private readonly GEDLoader _gedLoader;
        private readonly ReportWriter _reportWriter;
        private readonly ILogger<GenealogyOMatic> _logger;
        private readonly IConfiguration _configuration;
        private readonly string InputFilename;
        private readonly string OutputFilename;

        public GenealogyOMatic(ILogger<GenealogyOMatic> logger, IConfiguration configuration, GEDLoader gedLoader, ReportWriter reportWriter)
        {
            _logger = logger;
            _configuration = configuration;
            _gedLoader = gedLoader;
            _reportWriter = reportWriter;

             InputFilename = configuration["GenealogyHelper:InputFilename"];
            OutputFilename = configuration["GenealogyHelper:OutputFilename"];
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting application");
            
            _gedLoader.LoadGEDFile(InputFilename);
            _reportWriter.WriteReport(OutputFilename, _gedLoader.GEDModel);
            //_gedLoader.LoadGEDFile($"d:\\dev\\projects\\genealogyhelper\\test.ged");
            //_reportWriter.WriteReport(@"d:\dev\projects\genealogyhelper\output.csv", _gedLoader.GEDModel);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
