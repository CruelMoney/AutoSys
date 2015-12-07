using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Logic.StudyConfiguration.BiblographyParser;
using StudyConfigurationServer.Models;

namespace LogicTests1.BibTexParserTests
{
    [TestClass]
    public class ItemValidatorTest
    {
        Item _item;
        ItemValidator _iv;
        Dictionary<Item.ItemType, IItemChecker> _checkerDict;
        Dictionary<Item.FieldType, string> _fieldDict;
        IItemChecker _checker;

        [TestInitialize]
        public void Initialize()
        {
            _fieldDict = new Dictionary<Item.FieldType, string>();
            _checker = new DefaultItemChecker();
            _checkerDict = new Dictionary<Item.ItemType, IItemChecker>();
            _checkerDict.Add(Item.ItemType.Book, new DefaultItemChecker());
            _iv = new ItemValidator(_checkerDict); 
        } 


        [TestMethod]
        public void ItemsAllValidTest()
        { 
            _fieldDict.Add(Item.FieldType.Booktitle, "Hello World");
            _fieldDict.Add(Item.FieldType.Author, "Christopher");

            _item = new Item(Item.ItemType.Book, _fieldDict);

            Assert.IsTrue(_iv.IsItemValid(_item));
        }

        [TestMethod]
        public void ItemsSomeValidTest()
        {
            _fieldDict.Add(Item.FieldType.Booktitle, "Hello\n World");
            _fieldDict.Add(Item.FieldType.Author, "Christopher");

            _item = new Item(Item.ItemType.Book, _fieldDict);

            Assert.IsFalse(_iv.IsItemValid(_item));
        }

        [TestMethod]
        public void ItemsAllInvalidTest()
        {
            _fieldDict.Add(Item.FieldType.Booktitle, "Hello\n World");
            _fieldDict.Add(Item.FieldType.Author, "Christ\nopher");

            _item = new Item(Item.ItemType.Book, _fieldDict);

            Assert.IsFalse(_iv.IsItemValid(_item));
        }

        [TestMethod]
        public void ItemsValidWithoutDefinedCheckerTest()
        {
            _fieldDict.Add(Item.FieldType.Title, "Hello World");
            _fieldDict.Add(Item.FieldType.Author, "Christopher");

            _item = new Item(Item.ItemType.PhDThesis, _fieldDict);

            Assert.IsTrue(_iv.IsItemValid(_item));
        }

        [TestMethod]
        public void ItemsInvalidWithoutDefinedCheckerTest()
        {
            _fieldDict.Add(Item.FieldType.Title, "Hello\n World");
            _fieldDict.Add(Item.FieldType.Author, "Christopher");

            _item = new Item(Item.ItemType.PhDThesis, _fieldDict);

            Assert.IsFalse(_iv.IsItemValid(_item));
        }


    }
}
