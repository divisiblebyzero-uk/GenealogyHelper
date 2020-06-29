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
        private readonly string IndividualsOutputFilename;
        private readonly string EventsOutputFilename;
        private readonly string KeyIndividual;

        public GenealogyOMatic(ILogger<GenealogyOMatic> logger, IConfiguration configuration, GEDLoader gedLoader, ReportWriter reportWriter)
        {
            _logger = logger;
            _configuration = configuration;
            _gedLoader = gedLoader;
            _reportWriter = reportWriter;

            InputFilename = configuration["GenealogyHelper:InputFilename"];
            IndividualsOutputFilename = configuration["GenealogyHelper:IndividualsOutputFilename"];
            EventsOutputFilename = configuration["GenealogyHelper:EventsOutputFilename"];
            KeyIndividual = configuration["GenealogyHelper:KeyIndividual"];
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting application");
            
            _gedLoader.LoadGEDFile(InputFilename, KeyIndividual);
            _reportWriter.WriteReports(_gedLoader.GEDModel, IndividualsOutputFilename, EventsOutputFilename);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
