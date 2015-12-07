using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Logic.StudyConfiguration.BiblographyParser;
using StudyConfigurationServer.Models;

namespace LogicTests1.BibTexParserTests
{


    [TestClass]
    public class FieldValidatorTest
    {

        Dictionary<Item.FieldType, IFieldChecker> _checkerDict;
        IFieldChecker _checker;
        FieldValidator _fv;


        [TestInitialize]
        public void Initialize(){
            _checker = new DefaultFieldChecker();
            _checkerDict = new Dictionary<Item.FieldType, IFieldChecker>();
            _checkerDict.Add(Item.FieldType.Author, _checker);
            _fv = new FieldValidator(_checkerDict);
            }


        [TestMethod]
        public void FieldValidTest()
        {
            Assert.IsTrue(_fv.IsFieldValid("Christopher", Item.FieldType.Author));
        }

        [TestMethod]
        public void FieldinvalidTest()
        {
            Assert.IsFalse(_fv.IsFieldValid("Chris\ntopher", Item.FieldType.Author));
        }

        [TestMethod]
        public void FieldValidWithoutDefinedCheckerTest()
        {
            Assert.IsTrue(_fv.IsFieldValid("Hello World", Item.FieldType.Booktitle));
        }

        [TestMethod]
        public void FieldInvalidWithoutDefinedCheckerTest()
        {
            Assert.IsFalse(_fv.IsFieldValid("Hello\n World", Item.FieldType.Booktitle));
        }


    }
}
