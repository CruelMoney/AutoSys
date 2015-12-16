#region Using

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Logic.StudyManagement.BiblographyParser;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServerTests.UnitTests.StudyConfiguration.BibTexParserTests
{
    [TestClass]
    public class FieldValidatorTest
    {
        private IFieldChecker _checker;

        private Dictionary<FieldType, IFieldChecker> _checkerDict;
        private FieldValidator _fv;


        [TestInitialize]
        public void Initialize()
        {
            _checker = new DefaultFieldChecker();
            _checkerDict = new Dictionary<FieldType, IFieldChecker>();
            _checkerDict.Add(new FieldType {Type = FieldType.TypEField.Author}, _checker);
            _fv = new FieldValidator(_checkerDict);
        }


        [TestMethod]
        public void TestFieldValid()
        {
            Assert.IsTrue(_fv.IsFieldValid("Christopher", new FieldType {Type = FieldType.TypEField.Author}));
        }

        [TestMethod]
        public void TestFieldInvalid()
        {
            Assert.IsFalse(_fv.IsFieldValid("Chris\ntopher", new FieldType {Type = FieldType.TypEField.Author}));
        }

        [TestMethod]
        public void TestFieldValidWithoutDefinedChecker()
        {
            Assert.IsTrue(_fv.IsFieldValid("Hello World", new FieldType {Type = FieldType.TypEField.Booktitle}));
        }

        [TestMethod]
        public void TestFieldInvalidWithoutDefinedChecker()
        {
            Assert.IsFalse(_fv.IsFieldValid("Hello\n World", new FieldType {Type = FieldType.TypEField.Booktitle}));
        }
    }
}