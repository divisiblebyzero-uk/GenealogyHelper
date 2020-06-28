using System;
using System.Collections.Generic;
using System.Text;

namespace GenealogyHelper.Model
{
    public class Family
    {
        public string XrefId { get; set; }
        public string HusbandXrefId { get; set; }
        public string WifeXrefId { get; set; }
        public string PlaceOfWedding { get; set; }
    }
}
