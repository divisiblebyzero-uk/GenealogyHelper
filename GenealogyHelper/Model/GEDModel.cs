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
            Console.WriteLine("Hello");
        }
        public Dictionary<string, Individual> Individuals { get; set; }
    }
}
