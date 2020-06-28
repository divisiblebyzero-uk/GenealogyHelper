using System;
using System.Collections.Generic;
using System.Text;

namespace GenealogyHelper.Model
{
    public class GEDComLine
    {
        /**
         * From gedcom definition: https://edge.fscdn.org/assets/img/documents/ged551-5bac5e57fe88dd37df0e153d9c515335.pdf
         * A gedcom_line has the following syntax:
         * gedcom_line:=
         * level + delim + [optional_xref_ID] + tag + [optional_line_value] + terminator
         */

        private const string GedcomRegex = @"([0-9]+)\s*(@.*@\s*){0,1}([^\s]*)(.*)";
        //private const string GedcomRegex = @"([0-9])\s(.*)";
        public GEDComLine() { }

        public GEDComLine(string text)
        {
            foreach (System.Text.RegularExpressions.Match m in
                System.Text.RegularExpressions.Regex.Matches(text, GedcomRegex))
            {
                Level = Int32.Parse(m.Groups[1].Value);
                XrefId = m.Groups[2].Value.Trim();
                Tag = m.Groups[3].Value.Trim();
                LineValue = m.Groups[4].Value.Trim();
            }
        }

        public int Level { get; set; }
        public string XrefId { get; set; }
        public string Tag { get; set; }
        public string LineValue { get; set; }

        public override string ToString()
        {
            return $"{nameof(Level)}: {Level}, {nameof(XrefId)}: {XrefId}, {nameof(Tag)}: {Tag}, {nameof(LineValue)}: {LineValue}";
        }
    }
}
