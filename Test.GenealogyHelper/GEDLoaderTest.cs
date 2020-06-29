using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenealogyHelper.Model;
using GenealogyHelper.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using static Xunit.Assert;

namespace Test.GenealogyHelper
{
    public class GEDLoaderTest
    {

        private GEDModel GetModelFromTestInput()
        {
            var mockLogger = new Mock<ILogger<GEDLoader>>();

            GEDLoader loader = new GEDLoader(mockLogger.Object);
            loader.LoadGEDFile("./Resources/TestInput.ged", "@I0@");
            Equal(5, loader.GEDModel.Individuals.Count);
            True(loader.GEDModel.Individuals.ContainsKey("@I0@"));
            False(loader.GEDModel.Individuals.ContainsKey("MadeUpKey"));

            return loader.GEDModel;
        }

        [Fact]
        public void TestFileLoad()
        {
            var model = GetModelFromTestInput();

            Individual joe = model.Individuals["@I0@"];
            Equal("Joseph Kingston /Bloggs/", joe.Name);
            Equal("M", joe.Sex);
            Equal("3DE1240DC286491A8CA28975AC59D0AEA994", joe.Uid);
            Equal("Kingston, Jamaica", joe.PlaceOfBirth);
            Null(joe.PlaceOfDeath);

            Individual jane = model.Individuals["@I1@"];
            Equal("Jane Patience /Smith/", jane.Name);
            Equal("F", jane.Sex);
            Equal("D5D379E7F39348FEBFC22E8D865AF2CEB06C", jane.Uid);
            Equal("Kingston upon Hull, England", jane.PlaceOfBirth);
            Null(jane.PlaceOfDeath);

            Individual manchester = model.Individuals["@I5@"];
            Equal("Manchester /Black/", manchester.Name);
            Equal("M", manchester.Sex);
            Equal("1C720EB242B34CBB943672FE56ABACF92AF6", manchester.Uid);
            Equal("Manchester, UK", manchester.PlaceOfBirth);
            Equal("Manchester, UK", manchester.PlaceOfDeath);
            False(manchester.Principal);

            Family family = model.Families["@F370@"];
            Equal("@I0@", family.HusbandXrefId);
            Equal("@I1@", family.WifeXrefId);
            Equal("Gretna Green, UK", family.PlaceOfWedding);
            Contains(family.ChildrenXrefIds, x => x == "@I5@");

            Individual gerald = model.Individuals["@I2@"];
            Individual bernadette = model.Individuals["@I4@"];
            True(gerald.Principal);
            True(bernadette.Principal);
        }

        [Fact]
        public void TestEventList()
        {
            var events = GetModelFromTestInput().Events;
            var e = events.First(e => e.PlaceName == "Gretna Green, UK");
            Equal("Wedding", e.EventType);

        }
    }
}
