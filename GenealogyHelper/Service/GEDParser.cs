using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GenealogyHelper.Model;
using Microsoft.Extensions.Logging;

namespace GenealogyHelper.Service
{
    public class GEDParser
    {
        private ILogger<GEDParser> _logger;
        private GEDModel _gedModel;
        

        public GEDParser(ILogger<GEDParser> logger)
        {
            _logger = logger;
        }

        private void ParseLevel0(Queue<GEDComLine> queue)
        {
            while (queue.Count > 0)
            {
                GEDComLine line = queue.Peek();
                if (line.Level == 0)
                {
                    switch (line.Tag)
                    {
                        case "INDI":
                            Individual i = ParseIndividual(queue);
                            _gedModel.Individuals.Add(i.Id, i);
                            break;
                        default:
                            queue.Dequeue();
                            _logger.LogInformation($"Unknown level 0 type: {line}");
                            break;
                    }
                }
                else
                {
                    queue.Dequeue();
                    _logger.LogInformation($"Expecting level 0 type: {line}");
                }
            }
        }

        private Individual ParseIndividual(Queue<GEDComLine> queue)
        {
            GEDComLine line = queue.Dequeue();
            Individual i = new Individual();
            i.Id = line.XrefId;

            while (queue.Peek().Level == 1)
            {
                GEDComLine subinformation = queue.Dequeue();

                switch (subinformation.Tag)
                {
                    case "NAME":
                        i.Name = subinformation.LineValue;
                        break;
                    default:
                        _logger.LogInformation($"Unknown Individual information type: {subinformation.Tag}");
                        break;
                }
            }

            return i;
        }

        public void ParseGEDFile(string filename)
        {
            _gedModel = new GEDModel();
            try
            {
                var lines = File.ReadLines(filename);
                var arrayOfGedComLines = lines.Select(s => new GEDComLine(s)).ToArray();
                var queue = new Queue<GEDComLine>(arrayOfGedComLines);

                ParseLevel0(queue);
            }
            catch (IOException e)
            {
                _logger.LogError(e, "The file could not be read");
            }
            _logger.LogInformation($"Completed: {_gedModel}");
        }
    }
}
