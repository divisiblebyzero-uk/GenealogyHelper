using System;
using System.Collections.Generic;
using System.Text;

namespace GenealogyHelper.Model
{
    public class Individual
    {
        public string XrefId { get; set; }
        public string Uid { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string PlaceOfBirth { get; set; }
        public string PlaceOfDeath { get; set; }
        public string PlaceOfWedding { get; set; }
        public bool Principal { get; set; }
    }
}
