using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Logic.StudyConfiguration.BiblographyParser;

namespace LogicTests1.BibTexParserTests
{
    [TestClass]
    public class DefaultFieldCheckerTest
    {
        DefaultFieldChecker _DefaultFieldChecker;

        private const string _valid = "test";
        private const string _valid2 = "test spacing";
        private const string _valid3 = "t-szw!th§m()b*|s";
        private const string _valid4 = "1234";
        private const string _invalid = "test\ntest";
        private const string _invalid2 = "test \n test";
        private const string _invalid3 = "test test\n ";
        private const string _invalid4 = "\ntest test";

        [TestInitialize]
        public void initialize()
        {
            _DefaultFieldChecker = new DefaultFieldChecker();
            
        }

        [TestMethod]
        public void DefaultFieldValidTest()
        {

            Assert.IsTrue(_DefaultFieldChecker.Validate(_valid));
            Assert.IsTrue(_DefaultFieldChecker.Validate(_valid2));
            Assert.IsTrue(_DefaultFieldChecker.Validate(_valid3));
            Assert.IsTrue(_DefaultFieldChecker.Validate(_valid4));

        }

        [TestMethod]
        public void DefaultFieldInvalidTest()
        { 
            Assert.IsFalse(_DefaultFieldChecker.Validate(_invalid));
            Assert.IsFalse(_DefaultFieldChecker.Validate(_invalid2));
            Assert.IsFalse(_DefaultFieldChecker.Validate(_invalid3));
            Assert.IsFalse(_DefaultFieldChecker.Validate(_invalid4));
        }
    }
}
