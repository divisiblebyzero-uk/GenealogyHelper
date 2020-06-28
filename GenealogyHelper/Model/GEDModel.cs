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
            Console.WriteLine("Hello");
        }
        public Dictionary<string, Individual> Individuals { get; set; }
        public Dictionary<string, Family> Families { get; set; }
    }
}
