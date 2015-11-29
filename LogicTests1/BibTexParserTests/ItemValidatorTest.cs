using System.Collections.Generic;
using BibliographyParser;
using Logic.Model;
using Logic.StudyConfiguration.BiblographyParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicTests1.BibTexParserTests
{
    [TestClass]
    public class ItemValidatorTest
    {
        ItemLogic _item;
        ItemValidator _iv;
        Dictionary<ItemLogic.ItemType, IItemChecker> _checkerDict;
        Dictionary<ItemLogic.FieldType, string> _fieldDict;
        IItemChecker _checker;

        [TestInitialize]
        public void Initialize()
        {
            _fieldDict = new Dictionary<ItemLogic.FieldType, string>();
            _checker = new DefaultItemChecker();
            _checkerDict = new Dictionary<ItemLogic.ItemType, IItemChecker>();
            _checkerDict.Add(ItemLogic.ItemType.Book, new DefaultItemChecker());
            _iv = new ItemValidator(_checkerDict); 
        } 


        [TestMethod]
        public void allFieldsValid()
        { 
            _fieldDict.Add(ItemLogic.FieldType.Booktitle, "Hello World");
            _fieldDict.Add(ItemLogic.FieldType.Author, "Christopher");

            _item = new ItemLogic(ItemLogic.ItemType.Book, _fieldDict);

            Assert.IsTrue(_iv.IsItemValid(_item));
        }

        [TestMethod]
        public void someFieldsValid()
        {
            _fieldDict.Add(ItemLogic.FieldType.Booktitle, "Hello\n World");
            _fieldDict.Add(ItemLogic.FieldType.Author, "Christopher");

            _item = new ItemLogic(ItemLogic.ItemType.Book, _fieldDict);

            Assert.IsFalse(_iv.IsItemValid(_item));
        }

        [TestMethod]
        public void allFieldsInvalid()
        {
            _fieldDict.Add(ItemLogic.FieldType.Booktitle, "Hello\n World");
            _fieldDict.Add(ItemLogic.FieldType.Author, "Christ\nopher");

            _item = new ItemLogic(ItemLogic.ItemType.Book, _fieldDict);

            Assert.IsFalse(_iv.IsItemValid(_item));
        }

        [TestMethod]
        public void validWithoutDefinedChecker()
        {
            _fieldDict.Add(ItemLogic.FieldType.Title, "Hello World");
            _fieldDict.Add(ItemLogic.FieldType.Author, "Christopher");

            _item = new ItemLogic(ItemLogic.ItemType.PhDThesis, _fieldDict);

            Assert.IsTrue(_iv.IsItemValid(_item));
        }

        [TestMethod]
        public void invalidWithoutDefinedChecker()
        {
            _fieldDict.Add(ItemLogic.FieldType.Title, "Hello\n World");
            _fieldDict.Add(ItemLogic.FieldType.Author, "Christopher");

            _item = new ItemLogic(ItemLogic.ItemType.PhDThesis, _fieldDict);

            Assert.IsFalse(_iv.IsItemValid(_item));
        }


    }
}
