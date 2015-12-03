using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Logic.StudyConfiguration.BiblographyParser;
using StudyConfigurationServer.Models;

namespace LogicTests1.BibTexParserTests
{
    [TestClass]
    public class DefaultItemCheckerTest
    {

        Item _validItem;
        Item _invalidItem;
        DefaultItemChecker _dfc;

        Dictionary<Item.FieldType, string> _validDict = new Dictionary<Item.FieldType, string>();
        Dictionary<Item.FieldType, string> _invalidDict = new Dictionary<Item.FieldType, string>();
        DefaultItemChecker _DefaultItemChecker = new DefaultItemChecker();

        [TestInitialize]
        public void Initialize(){
            _validDict.Add(Item.FieldType.Booktitle, "Hello World");
            _validDict.Add(Item.FieldType.Author, "Christopher");

            _invalidDict.Add(Item.FieldType.Booktitle, "Hello World");
            _invalidDict.Add(Item.FieldType.Author, "invalid\n field");

            _validItem = new Item(Item.ItemType.Book, _validDict);
            _invalidItem = new Item(Item.ItemType.Book, _invalidDict);
            }


        [TestMethod]
        public void TestValid()
        {
            Assert.IsTrue(_DefaultItemChecker.Validate(_validItem));
        }

        [TestMethod]
        public void TestInValid()
        {
            Assert.IsFalse(_DefaultItemChecker.Validate(_invalidItem));
        }
    }
}
