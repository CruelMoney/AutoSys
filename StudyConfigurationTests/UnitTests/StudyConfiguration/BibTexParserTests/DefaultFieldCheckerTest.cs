#region Using

using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Logic.StudyManagement.BiblographyParser;

#endregion

namespace StudyConfigurationServerTests.UnitTests.StudyConfiguration.BibTexParserTests
{
    [TestClass]
    public class DefaultFieldCheckerTest
    {
        private const string Valid = "test";
        private const string Valid2 = "test spacing";
        private const string Valid3 = "t-szw!th§m()b*|s";
        private const string Valid4 = "1234";
        private const string Invalid = "test\ntest";
        private const string Invalid2 = "test \n test";
        private const string Invalid3 = "test test\n ";
        private const string Invalid4 = "\ntest test";
        private DefaultFieldChecker _defaultFieldChecker;

        [TestInitialize]
        public void Initialize()
        {
            _defaultFieldChecker = new DefaultFieldChecker();
        }

        [TestMethod]
        public void TestDefaultFieldValid()
        {
            Assert.IsTrue(_defaultFieldChecker.Validate(Valid));
            Assert.IsTrue(_defaultFieldChecker.Validate(Valid2));
            Assert.IsTrue(_defaultFieldChecker.Validate(Valid3));
            Assert.IsTrue(_defaultFieldChecker.Validate(Valid4));
        }

        [TestMethod]
        public void TestDefaultFieldInvalid()
        {
            Assert.IsFalse(_defaultFieldChecker.Validate(Invalid));
            Assert.IsFalse(_defaultFieldChecker.Validate(Invalid2));
            Assert.IsFalse(_defaultFieldChecker.Validate(Invalid3));
            Assert.IsFalse(_defaultFieldChecker.Validate(Invalid4));
        }
    }
}