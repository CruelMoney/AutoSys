#region Using

using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Logic.StudyManagement.BiblographyParser;
using StudyConfigurationServer.Logic.StudyManagement.BiblographyParser.bibTex;
using StudyConfigurationServer.Models;
using StudyConfigurationServerTests.Properties;

#endregion

namespace StudyConfigurationServerTests.UnitTests.StudyConfiguration.BibTexParserTests
{
    [TestClass]
    public class BibTexParserTest
    {
        private const string ValidItem = "@INPROCEEDINGS{839269, author = {Hilburn, T.B.and Bagert, D.J.},}";
        private const string InvalidItemType = "@INVALIDITEMTYPE{839269, author = {Hilburn, T.B.and Bagert, D.J.},}";
        private const string InvalidItemKey = "@INPROCEEDINGS{invalid author = {Hilburn, T.B.and Bagert, D.J.},}";

        private const string InvalidFieldType =
            "@INPROCEEDINGS{839269, invalidField = {Hilburn, T.B.and Bagert, D.J.},}";

        private const string InvalidItemSyntax = "@INPROCEEDINGS{1158672, author={Pour, G.}";
        private const string ValidItem2 = "@INPROCEEDINGS{1158672, author={Pour, G.},}";

        [TestMethod]
        public void TestParseBibtex()
        {
            var file = Resources.bibtex;

            var fileString = Encoding.Default.GetString(file);

            var parser = new BibTexParser(new ItemValidator());
            var bib = parser.Parse(fileString);

            Assert.AreEqual(23, bib.Count);
        }

        [TestMethod]
        public void TestParseValidItem()
        {
            var parser = new BibTexParser(new ItemValidator());
            var bib = parser.Parse(ValidItem);

            var item = bib[0];

            Assert.AreEqual(1, bib.Count);
            Assert.AreEqual(Item.ItemType.InProceedings, item.Type);
            Assert.AreEqual("Hilburn, T.B.and Bagert, D.J.", item.FindFieldValue("Author"));
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidDataException))]
        public void TestParseInValidItemType()
        {
            var parser = new BibTexParser(new ItemValidator());
            var bib = parser.Parse(InvalidItemType);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidDataException))]
        public void TestParseInValidFieldType()
        {
            var parser = new BibTexParser(new ItemValidator());
            var bib = parser.Parse(InvalidFieldType);
        }


        [TestMethod]
        public void TestParseInValidItemSyntax()
        {
            var parser = new BibTexParser(new ItemValidator());
            var bib = parser.Parse(InvalidItemSyntax);

            Assert.AreEqual(0, bib.Count);
        }


        [TestMethod]
        public void TestParseMultipleItems()
        {
            var parser = new BibTexParser(new ItemValidator());
            var bib = parser.Parse(ValidItem);
            bib.AddRange(parser.Parse(ValidItem2));


            Assert.AreEqual(2, bib.Count);
            Assert.AreEqual("Hilburn, T.B.and Bagert, D.J.", bib[0].FindFieldValue("Author"));
            Assert.AreEqual("Pour, G.", bib[1].FindFieldValue("Author"));
        }

        [TestMethod]
        public void TestParseValidAndInvalid()
        {
            var parser = new BibTexParser(new ItemValidator());
            var bib = parser.Parse(ValidItem);
            bib.AddRange(parser.Parse(InvalidItemKey));


            Assert.AreEqual(1, bib.Count);
            Assert.AreEqual("Hilburn, T.B.and Bagert, D.J.", bib[0].FindFieldValue("Author"));
        }
    }
}