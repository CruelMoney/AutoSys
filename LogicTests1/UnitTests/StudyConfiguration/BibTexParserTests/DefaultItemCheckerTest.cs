#region Using

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Logic.StudyConfiguration.BiblographyParser;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServerTests.UnitTests.StudyConfiguration.BibTexParserTests
{
    [TestClass]
    public class DefaultItemCheckerTest
    {
        private readonly DefaultItemChecker _defaultItemChecker = new DefaultItemChecker();
        private DefaultItemChecker _dfc;
        private readonly Dictionary<FieldType, string> _invalidDict = new Dictionary<FieldType, string>();
        private Item _invalidItem;

        private readonly Dictionary<FieldType, string> _validDict = new Dictionary<FieldType, string>();

        private Item _validItem;

        [TestInitialize]
        public void Initialize()
        {
            _validDict.Add(new FieldType {Type = FieldType.TypEField.Booktitle}, "Hello World");
            _validDict.Add(new FieldType {Type = FieldType.TypEField.Author}, "Christopher");

            _invalidDict.Add(new FieldType {Type = FieldType.TypEField.Booktitle}, "Hello World");
            _invalidDict.Add(new FieldType {Type = FieldType.TypEField.Author}, "invalid\n field");

            _validItem = new Item(Item.ItemType.Book, _validDict);
            _invalidItem = new Item(Item.ItemType.Book, _invalidDict);
        }


        [TestMethod]
        public void TestItemValid()
        {
            Assert.IsTrue(_defaultItemChecker.Validate(_validItem));
        }

        [TestMethod]
        public void TestItemInValid()
        {
            Assert.IsFalse(_defaultItemChecker.Validate(_invalidItem));
        }
    }
}