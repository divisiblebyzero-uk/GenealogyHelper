using System;
using System.Collections.Generic;
using System.Text;

namespace GenealogyHelper.Model
{
    public class Event
    {
        public string PlaceName { get; set; }
        public string EventType { get; set; }
        public string Subject1 { get; set; }
        public string Subject2 { get; set; }
        public string Notes { get; set; }
        public bool Principal { get; set; }
    }
}
