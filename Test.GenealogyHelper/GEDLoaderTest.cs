using System;
using System.Collections.Generic;
using System.Text;
using GenealogyHelper.Model;
using GenealogyHelper.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Test.GenealogyHelper
{
    public class GEDLoaderTest
    {
        [Fact]
        public void TestFileLoad()
        {
            var mockLogger = new Mock<ILogger<GEDLoader>>();

            GEDLoader loader = new GEDLoader(mockLogger.Object);
            loader.LoadGEDFile("./Resources/TestInput.ged");
            Assert.Equal(3, loader.GEDModel.Individuals.Count);
            Assert.True(loader.GEDModel.Individuals.ContainsKey("@I0@"));
            Assert.False(loader.GEDModel.Individuals.ContainsKey("MadeUpKey"));

            Individual joe = loader.GEDModel.Individuals["@I0@"];
            Assert.Equal("Joseph Kingston /Bloggs/", joe.Name);
            Assert.Equal("M", joe.Sex);
            Assert.Equal("3DE1240DC286491A8CA28975AC59D0AEA994", joe.Uid);
            Assert.Equal("Kingston, Jamaica", joe.PlaceOfBirth);
            Assert.Null(joe.PlaceOfDeath);

            Individual jane = loader.GEDModel.Individuals["@I1@"];
            Assert.Equal("Jane Patience /Smith/", jane.Name);
            Assert.Equal("F", jane.Sex);
            Assert.Equal("D5D379E7F39348FEBFC22E8D865AF2CEB06C", jane.Uid);
            Assert.Equal("Kingston upon Hull, England", jane.PlaceOfBirth);
            Assert.Null(jane.PlaceOfDeath);

            Individual manchester = loader.GEDModel.Individuals["@I5@"];
            Assert.Equal("Manchester /Black/", manchester.Name);
            Assert.Equal("M", manchester.Sex);
            Assert.Equal("1C720EB242B34CBB943672FE56ABACF92AF6", manchester.Uid);
            Assert.Equal("Manchester, UK", manchester.PlaceOfBirth);
            Assert.Equal("Manchester, UK", manchester.PlaceOfDeath);

            Family family = loader.GEDModel.Families["@F370@"];
            Assert.Equal("@I0@", family.HusbandXrefId);
            Assert.Equal("@I1@", family.WifeXrefId);
            Assert.Equal("Gretna Green, UK", family.PlaceOfWedding);
        }
    }
}
