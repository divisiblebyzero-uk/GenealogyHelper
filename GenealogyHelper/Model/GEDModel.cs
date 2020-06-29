using System;
using System.Collections.Generic;
using System.Text;

namespace GenealogyHelper.Model
{
    public class GEDModel
    {
        public GEDModel()
        {
            Individuals = new Dictionary<string, Individual>();
            Families = new Dictionary<string, Family>();
            Events = new List<Event>();
        }
        public Dictionary<string, Individual> Individuals { get; set; }
        public Dictionary<string, Family> Families { get; set; }
        public List<Event> Events { get; set; }
    }
}
