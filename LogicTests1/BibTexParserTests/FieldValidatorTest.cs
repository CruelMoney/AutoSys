using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Logic.StudyConfiguration.BiblographyParser;
using StudyConfigurationServer.Models;

namespace LogicTests1.BibTexParserTests
{


    [TestClass]
    public class FieldValidatorTest
    {

        Dictionary<FieldType, IFieldChecker> _checkerDict;
        IFieldChecker _checker;
        FieldValidator _fv;


        [TestInitialize]
        public void Initialize(){
            _checker = new DefaultFieldChecker();
            _checkerDict = new Dictionary<FieldType, IFieldChecker>();
            _checkerDict.Add(new FieldType() { Type = FieldType.TypEField.Author}, _checker);
            _fv = new FieldValidator(_checkerDict);
            }


        [TestMethod]
        public void TestFieldValid()
        {
            Assert.IsTrue(_fv.IsFieldValid("Christopher", new FieldType() { Type = FieldType.TypEField.Author}));
        }

        [TestMethod]
        public void TestFieldInvalid()
        {
            Assert.IsFalse(_fv.IsFieldValid("Chris\ntopher", new FieldType() { Type = FieldType.TypEField.Author}));
        }

        [TestMethod]
        public void TestFieldValidWithoutDefinedChecker()
        {
            Assert.IsTrue(_fv.IsFieldValid("Hello World", new FieldType() { Type = FieldType.TypEField.Booktitle}));
        }

        [TestMethod]
        public void TestFieldInvalidWithoutDefinedChecker()
        {
            Assert.IsFalse(_fv.IsFieldValid("Hello\n World", new FieldType() { Type = FieldType.TypEField.Booktitle}));
        }


    }
}
