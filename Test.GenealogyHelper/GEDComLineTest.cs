using System;
using GenealogyHelper.Model;
using Xunit;

namespace Test.GenealogyHelper
{
    public class GEDComLineTest
    {
        [Theory]
        [InlineData("0 HEAD", 0, "", "HEAD", "")]
        [InlineData("1 DATE 26 JUN 2020", 1, "", "DATE", "26 JUN 2020")]
        [InlineData("0 @I1@ INDI", 0, "@I1@", "INDI", "")]
        public void TestParsing(string input, int level, string xrefId, string tag, string lineValue)
        {
            var line = new GEDComLine(input);
            Assert.Equal(level, line.Level);
            Assert.Equal(xrefId, line.XrefId);
            Assert.Equal(tag, line.Tag);
            Assert.Equal(lineValue, line.LineValue);
        }
    } 
}
