#region Using

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Logic.StudyManagement.BiblographyParser;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServerTests.UnitTests.StudyConfiguration.BibTexParserTests
{
    [TestClass]
    public class ItemValidatorTest
    {
        private IItemChecker _checker;
        private Dictionary<Item.ItemType, IItemChecker> _checkerDict;
        private Dictionary<FieldType, string> _fieldDict;
        private Item _item;
        private ItemValidator _iv;

        [TestInitialize]
        public void Initialize()
        {
            _fieldDict = new Dictionary<FieldType, string>();
            _checker = new DefaultItemChecker();
            _checkerDict = new Dictionary<Item.ItemType, IItemChecker>();
            _checkerDict.Add(Item.ItemType.Book, new DefaultItemChecker());
            _iv = new ItemValidator(_checkerDict);
        }


        [TestMethod]
        public void TestItemsAllValid()
        {
            _fieldDict.Add(new FieldType {Type = FieldType.TypEField.Booktitle}, "Hello World");
            _fieldDict.Add(new FieldType {Type = FieldType.TypEField.Author}, "Christopher");

            _item = new Item(Item.ItemType.Book, _fieldDict);

            Assert.IsTrue(_iv.IsItemValid(_item));
        }

        [TestMethod]
        public void TestItemsSomeValid()
        {
            _fieldDict.Add(new FieldType {Type = FieldType.TypEField.Booktitle}, "Hello\n World");
            _fieldDict.Add(new FieldType {Type = FieldType.TypEField.Author}, "Christopher");

            _item = new Item(Item.ItemType.Book, _fieldDict);

            Assert.IsFalse(_iv.IsItemValid(_item));
        }

        [TestMethod]
        public void TestItemsAllInvalid()
        {
            _fieldDict.Add(new FieldType {Type = FieldType.TypEField.Booktitle}, "Hello\n World");
            _fieldDict.Add(new FieldType {Type = FieldType.TypEField.Author}, "Christ\nopher");

            _item = new Item(Item.ItemType.Book, _fieldDict);

            Assert.IsFalse(_iv.IsItemValid(_item));
        }

        [TestMethod]
        public void TestItemsValidWithoutDefinedChecker()
        {
            _fieldDict.Add(new FieldType {Type = FieldType.TypEField.Title}, "Hello World");
            _fieldDict.Add(new FieldType {Type = FieldType.TypEField.Author}, "Christopher");

            _item = new Item(Item.ItemType.PhDThesis, _fieldDict);

            Assert.IsTrue(_iv.IsItemValid(_item));
        }

        [TestMethod]
        public void TestItemsInvalidWithoutDefinedChecker()
        {
            _fieldDict.Add(new FieldType {Type = FieldType.TypEField.Title}, "Hello\n World");
            _fieldDict.Add(new FieldType {Type = FieldType.TypEField.Author}, "Christopher");

            _item = new Item(Item.ItemType.PhDThesis, _fieldDict);

            Assert.IsFalse(_iv.IsItemValid(_item));
        }
    }
}