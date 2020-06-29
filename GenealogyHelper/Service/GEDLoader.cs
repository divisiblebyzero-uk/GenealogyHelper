using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GenealogyHelper.Model;
using Microsoft.Extensions.Logging;

namespace GenealogyHelper.Service
{
    public class GEDLoader
    {
        private readonly ILogger<GEDLoader> _logger;
        private readonly GEDModel _gedModel;

        public GEDModel GEDModel => _gedModel;

        public GEDLoader(ILogger<GEDLoader> logger)
        {
            _logger = logger;
            _gedModel = new GEDModel();
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
                            _gedModel.Individuals.Add(i.XrefId, i);
                            break;
                        case "FAM":
                            Family f = ParseFamily(queue);
                            _gedModel.Families.Add(f.XrefId, f);
                            break;
                        default:
                            queue.Dequeue();
                            _logger.LogDebug($"Unknown level 0 type: {line}");
                            while (queue.Count > 0 && queue.Peek().Level > 0)
                            {
                                queue.Dequeue();
                            }
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
            Individual i = new Individual
            {
                XrefId = line.XrefId
            };

            while (queue.Count > 0 && queue.Peek().Level >= 1)
            {
                GEDComLine subinformation = queue.Dequeue();

                switch (subinformation.Tag)
                {
                    case "NAME":
                        i.Name = subinformation.LineValue;
                        break;
                    case "SEX":
                        i.Sex = subinformation.LineValue;
                        break;
                    case "_UID":
                        i.Uid = subinformation.LineValue;
                        break;
                    case "BIRT":
                        while (queue.Count > 0 && queue.Peek().Level >= 2)
                        {
                            GEDComLine level2Information = queue.Dequeue();
                            switch (level2Information.Tag)
                            {
                                case "PLAC":
                                    i.PlaceOfBirth = level2Information.LineValue;
                                    break;
                                default:
                                    _logger.LogDebug($"Unknown Birth information type: {level2Information.Tag}");
                                    break;
                            }
                        }

                        break;
                    case "DEAT":
                        while (queue.Count > 0 && queue.Peek().Level >= 2)
                        {
                            GEDComLine level2Information = queue.Dequeue();
                            switch (level2Information.Tag)
                            {
                                case "PLAC":
                                    i.PlaceOfDeath = level2Information.LineValue;
                                    break;
                                default:
                                    _logger.LogDebug($"Unknown Death information type: {level2Information.Tag}");
                                    break;
                            }
                        }

                        break;
                    default:
                        _logger.LogDebug($"Unknown Individual information type: {subinformation.Tag}");
                        break;
                }
            }

            return i;
        }

        private Family ParseFamily(Queue<GEDComLine> queue)
        {
            GEDComLine line = queue.Dequeue();
            Family f = new Family
            {
                XrefId = line.XrefId
            };

            while (queue.Count > 0 && queue.Peek().Level >= 1)
            {
                GEDComLine subinformation = queue.Dequeue();

                switch (subinformation.Tag)
                {
                    case "HUSB":
                        f.HusbandXrefId = subinformation.LineValue;
                        break;
                    case "WIFE":
                        f.WifeXrefId = subinformation.LineValue;
                        break;
                    case "MARR":
                        while (queue.Count > 0 && queue.Peek().Level >= 2)
                        {
                            GEDComLine level2Information = queue.Dequeue();
                            switch (level2Information.Tag)
                            {
                                case "PLAC":
                                    f.PlaceOfWedding = level2Information.LineValue;
                                    break;
                                default:
                                    _logger.LogDebug($"Unknown Marriage information type: {level2Information.Tag}");
                                    break;
                            }
                        }

                        break;
                    default:
                        _logger.LogDebug($"Unknown Family information type: {subinformation.Tag}");
                        break;
                }
            }

            return f;
        }

        private void UpdateFamilyReferences()
        {
            foreach (var family in _gedModel.Families.Values)
            {
                if (family.PlaceOfWedding != null)
                {
                    if (family.HusbandXrefId != null && _gedModel.Individuals.ContainsKey(family.HusbandXrefId))
                    {
                        _gedModel.Individuals[family.HusbandXrefId].PlaceOfWedding = family.PlaceOfWedding;
                    }
                    if (family.WifeXrefId != null && _gedModel.Individuals.ContainsKey(family.WifeXrefId))
                    {
                        _gedModel.Individuals[family.WifeXrefId].PlaceOfWedding = family.PlaceOfWedding;
                    }
                }
            }
        }

        private void CreateEventList()
        {
            foreach (var individual in _gedModel.Individuals.Values)
            {
                if (!string.IsNullOrEmpty(individual.PlaceOfBirth))
                {
                    _gedModel.Events.Add(new Event
                    {
                        PlaceName = individual.PlaceOfBirth,
                        EventType = "Birth",
                        Subject1 = individual.XrefId
                    });
                }

                if (!string.IsNullOrEmpty(individual.PlaceOfDeath))
                {
                    _gedModel.Events.Add(new Event
                    {
                        PlaceName = individual.PlaceOfDeath,
                        EventType = "Death",
                        Subject1 = individual.XrefId
                    });
                }
            }

            foreach (var family in _gedModel.Families.Values)
            {
                if (!string.IsNullOrEmpty(family.PlaceOfWedding))
                {
                    _gedModel.Events.Add(new Event
                    {
                        PlaceName = family.PlaceOfWedding,
                        EventType = "Wedding",
                        Subject1 = family.HusbandXrefId,
                        Subject2 = family.WifeXrefId
                    });
                }
            }
        }

        public void LoadGEDFile(string filename)
        {
            if (File.Exists(filename))
            {
                LoadGEDData(File.ReadLines(filename));
            }
            else
            {
                _logger.LogError($"Error - input {filename} not found");
            }
            
        }

        public void LoadGEDData(IEnumerable<string> data)
        {
            var arrayOfGedComLines = data.Select(s => new GEDComLine(s)).ToArray();
            var queue = new Queue<GEDComLine>(arrayOfGedComLines);

            ParseLevel0(queue);

            UpdateFamilyReferences();

            CreateEventList();

            _logger.LogInformation($"Completed: loaded {_gedModel.Individuals.Count} individuals.");
        }
    }
}
