using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Models;

namespace LogicTests1.Model
{
    [TestClass]
    public class UserDataTests
    {
        UserData completedData;
        UserData completedData2;
        UserData incompleteData1;
        UserData incompleteData2;

        [TestInitialize]
        public void Initialize()
        {
            var completeData = new List<StoredString>() { new StoredString() { Value = "testData" }};
            var completeData2 = new List<StoredString>() { new StoredString() { Value = ""}, new StoredString() { Value = ""}, new StoredString() { Value = "dataTest"}, new StoredString() { Value = "" }};
            var nullData = new List<StoredString>();
            var emptyStringsData = new List<StoredString>() { new StoredString() { Value = ""}, new StoredString() { Value = ""}, new StoredString() { Value = ""} };

            completedData = new UserData() { Data = completeData };
            completedData2 = new UserData() { Data = completeData2 };
            incompleteData1 = new UserData() { Data = nullData };
            incompleteData2 = new UserData() { Data = emptyStringsData };
        }

        [TestMethod]
        public void TestUserDataEnteredTrue()
        {
            Assert.IsTrue(completedData.ContainsData());
            Assert.IsTrue(completedData2.ContainsData());
        }

        [TestMethod]
        public void TestUserDataTaskUnFinished()
        {
            Assert.IsFalse(incompleteData1.ContainsData());
            Assert.IsFalse(incompleteData2.ContainsData());
        }

    }
}
