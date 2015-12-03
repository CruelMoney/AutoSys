using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicTests1.BibTexParserTests
{
    [TestClass]
    public class BibTexParserTest
    {
        private const string _validItem = "@INPROCEEDINGS{839269, author = {Hilburn, T.B.and Bagert, D.J.},}";
        private const string _invalidItemType = "@INVALIDITEMTYPE{839269, author = {Hilburn, T.B.and Bagert, D.J.},}";
        private const string _invalidItemKey = "@INPROCEEDINGS{invalid author = {Hilburn, T.B.and Bagert, D.J.},}";
        private const string _invalidFieldType = "@INPROCEEDINGS{839269, invalidField = {Hilburn, T.B.and Bagert, D.J.},}";
        private const string _invalidItemSyntax = "@INPROCEEDINGS{1158672, author={Pour, G.}";
        private const string _validItem2 = "@INPROCEEDINGS{1158672, author={Pour, G.},}";
       
        [TestMethod]
        public void BibParserTest()
        {
            var _file = Properties.Resources.bibtex;
                
            var _fileString = System.Text.Encoding.Default.GetString(_file);

            var _parser = new Logic.StudyConfiguration.BiblographyParser.bibTex.BibTexParser(new ItemValidator());
            var _bib = _parser.Parse(_fileString);

            Assert.AreEqual(23, _bib.Count);
        }

        [TestMethod]
        public void parsingValidItem()
        {
            var _parser = new Logic.StudyConfiguration.BiblographyParser.bibTex.BibTexParser(new ItemValidator());
            var _bib = _parser.Parse(_validItem);
            
            var _item = _bib[0];

            Assert.AreEqual(1, _bib.Count);
            Assert.AreEqual(Item.ItemType.InProceedings, _item.Type);
            Assert.AreEqual("Hilburn, T.B.and Bagert, D.J.", _item.Fields[Item.FieldType.Author]);
    
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void parsingInValidItemType()
        {
            var _parser = new Logic.StudyConfiguration.BiblographyParser.bibTex.BibTexParser(new ItemValidator());
            var _bib = _parser.Parse(_invalidItemType);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void parsingInValidFieldType()
        {
            var _parser = new Logic.StudyConfiguration.BiblographyParser.bibTex.BibTexParser(new ItemValidator());
            var _bib = _parser.Parse(_invalidFieldType);
        }

  
        [TestMethod]
        public void parsingInValidItemSyntax()
        {
            var _parser = new Logic.StudyConfiguration.BiblographyParser.bibTex.BibTexParser(new ItemValidator());
            var _bib = _parser.Parse(_invalidItemSyntax);

            Assert.AreEqual(0, _bib.Count);
        }



        [TestMethod]
        public void parsingMultipleItems()
        {
            var _parser = new Logic.StudyConfiguration.BiblographyParser.bibTex.BibTexParser(new ItemValidator());
            var _bib = _parser.Parse(_validItem);
            _bib.AddRange(_parser.Parse(_validItem2));
            

            Assert.AreEqual(2, _bib.Count);
            Assert.AreEqual("Hilburn, T.B.and Bagert, D.J.", _bib[0].Fields[Item.FieldType.Author]);
            Assert.AreEqual("Pour, G.", _bib[1].Fields[Item.FieldType.Author]);

        }

        [TestMethod]
        public void parsingValidAndInvalid()
        {
            var _parser = new Logic.StudyConfiguration.BiblographyParser.bibTex.BibTexParser(new ItemValidator());
            var _bib = _parser.Parse(_validItem);
            _bib.AddRange(_parser.Parse(_invalidItemKey));


            Assert.AreEqual(1, _bib.Count);
            Assert.AreEqual("Hilburn, T.B.and Bagert, D.J.", _bib[0].Fields[Item.FieldType.Author]);
        }

  
    }
}