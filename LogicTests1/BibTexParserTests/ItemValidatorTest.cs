﻿using System.Collections.Generic;
using BibliographyParser;
using Logic.Model;
using Logic.StudyConfiguration.BiblographyParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void allFieldsValid()
        { 
            _fieldDict.Add(Item.FieldType.Booktitle, "Hello World");
            _fieldDict.Add(Item.FieldType.Author, "Christopher");

            _item = new Item(Item.ItemType.Book, _fieldDict);

            Assert.IsTrue(_iv.IsItemValid(_item));
        }

        [TestMethod]
        public void someFieldsValid()
        {
            _fieldDict.Add(Item.FieldType.Booktitle, "Hello\n World");
            _fieldDict.Add(Item.FieldType.Author, "Christopher");

            _item = new Item(Item.ItemType.Book, _fieldDict);

            Assert.IsFalse(_iv.IsItemValid(_item));
        }

        [TestMethod]
        public void allFieldsInvalid()
        {
            _fieldDict.Add(Item.FieldType.Booktitle, "Hello\n World");
            _fieldDict.Add(Item.FieldType.Author, "Christ\nopher");

            _item = new Item(Item.ItemType.Book, _fieldDict);

            Assert.IsFalse(_iv.IsItemValid(_item));
        }

        [TestMethod]
        public void validWithoutDefinedChecker()
        {
            _fieldDict.Add(Item.FieldType.Title, "Hello World");
            _fieldDict.Add(Item.FieldType.Author, "Christopher");

            _item = new Item(Item.ItemType.PhDThesis, _fieldDict);

            Assert.IsTrue(_iv.IsItemValid(_item));
        }

        [TestMethod]
        public void invalidWithoutDefinedChecker()
        {
            _fieldDict.Add(Item.FieldType.Title, "Hello\n World");
            _fieldDict.Add(Item.FieldType.Author, "Christopher");

            _item = new Item(Item.ItemType.PhDThesis, _fieldDict);

            Assert.IsFalse(_iv.IsItemValid(_item));
        }


    }
}
