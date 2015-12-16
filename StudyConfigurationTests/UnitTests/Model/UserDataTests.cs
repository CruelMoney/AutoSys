#region Using

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServerTests.UnitTests.Model
{
    [TestClass]
    public class UserDataTests
    {
        private UserData _completedData;
        private UserData _completedData2;
        private UserData _incompleteData1;
        private UserData _incompleteData2;

        [TestInitialize]
        public void Initialize()
        {
            var completeData = new List<StoredString> {new StoredString {Value = "testData"}};
            var completeData2 = new List<StoredString>
            {
                new StoredString {Value = ""},
                new StoredString {Value = ""},
                new StoredString {Value = "dataTest"},
                new StoredString {Value = ""}
            };
            var nullData = new List<StoredString>();
            var emptyStringsData = new List<StoredString>
            {
                new StoredString {Value = ""},
                new StoredString {Value = ""},
                new StoredString {Value = ""}
            };

            _completedData = new UserData {Data = completeData};
            _completedData2 = new UserData {Data = completeData2};
            _incompleteData1 = new UserData {Data = nullData};
            _incompleteData2 = new UserData {Data = emptyStringsData};
        }

        [TestMethod]
        public void TestUserDataEnteredTrue()
        {
            Assert.IsTrue(_completedData.ContainsData());
            Assert.IsTrue(_completedData2.ContainsData());
        }

        [TestMethod]
        public void TestUserDataTaskUnFinished()
        {
            Assert.IsFalse(_incompleteData1.ContainsData());
            Assert.IsFalse(_incompleteData2.ContainsData());
        }
    }
}