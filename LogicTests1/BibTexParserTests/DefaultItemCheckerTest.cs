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

        Dictionary<FieldType, string> _validDict = new Dictionary<FieldType, string>();
        Dictionary<FieldType, string> _invalidDict = new Dictionary<FieldType, string>();
        DefaultItemChecker _DefaultItemChecker = new DefaultItemChecker();

        [TestInitialize]
        public void Initialize(){
            _validDict.Add(new FieldType() { Type = FieldType.TypEField.Booktitle}, "Hello World");
            _validDict.Add(new FieldType() { Type = FieldType.TypEField.Author}, "Christopher");

            _invalidDict.Add(new FieldType() { Type = FieldType.TypEField.Booktitle}, "Hello World");
            _invalidDict.Add(new FieldType() { Type = FieldType.TypEField.Author}, "invalid\n field");

            _validItem = new Item(Item.ItemType.Book, _validDict);
            _invalidItem = new Item(Item.ItemType.Book, _invalidDict);
            }


        [TestMethod]
        public void TestItemValid()
        {
            Assert.IsTrue(_DefaultItemChecker.Validate(_validItem));
        }

        [TestMethod]
        public void TestItemInValid()
        {
            Assert.IsFalse(_DefaultItemChecker.Validate(_invalidItem));
        }
    }
}
