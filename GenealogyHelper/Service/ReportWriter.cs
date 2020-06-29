using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using CsvHelper;
using GenealogyHelper.Model;
using Microsoft.Extensions.Logging;

namespace GenealogyHelper.Service
{
    public class ReportWriter
    {
        private readonly ILogger<ReportWriter> _logger;

        public ReportWriter(ILogger<ReportWriter> logger)
        {
            _logger = logger;
        }

        public void WriteReports(GEDModel gedModel, string individualsFilename, string eventsFilename)
        {
            using (var writer = new StreamWriter(individualsFilename))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteHeader<Individual>();
                csv.NextRecord();
                foreach (Individual i in gedModel.Individuals.Values)
                {
                    csv.WriteRecord<Individual>(i);
                    csv.NextRecord();
                }
            }

            using (var writer = new StreamWriter(eventsFilename))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteHeader<Event>();
                csv.NextRecord();
                foreach (Event e in gedModel.Events)
                {
                    csv.WriteRecord<Event>(e);
                    csv.NextRecord();
                }
            }

        }
    }
}
