using System.Collections.Generic;
using Logic.Model;
using Logic.StudyConfiguration.BiblographyParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicTests1.BibTexParserTests
{
    [TestClass]
    public class DefaultItemCheckerTest
    {

        ItemLogic _validItem;
        ItemLogic _invalidItem;
        DefaultItemChecker _dfc;

        Dictionary<ItemLogic.FieldType, string> _validDict = new Dictionary<ItemLogic.FieldType, string>();
        Dictionary<ItemLogic.FieldType, string> _invalidDict = new Dictionary<ItemLogic.FieldType, string>();
        DefaultItemChecker _DefaultItemChecker = new DefaultItemChecker();

        [TestInitialize]
        public void Initialize(){
            _validDict.Add(ItemLogic.FieldType.Booktitle, "Hello World");
            _validDict.Add(ItemLogic.FieldType.Author, "Christopher");

            _invalidDict.Add(ItemLogic.FieldType.Booktitle, "Hello World");
            _invalidDict.Add(ItemLogic.FieldType.Author, "invalid\n field");

            _validItem = new ItemLogic(ItemLogic.ItemType.Book, _validDict);
            _invalidItem = new ItemLogic(ItemLogic.ItemType.Book, _invalidDict);
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
