using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Model.Tests
{
    [TestClass()]
    public class UserLogicTests
    {
        [TestMethod()]
        public void CorrectUserCreationTest()
        {
            var testUser = new UserLogic();
            testUser.Id = 1;
            testUser.Name = "testName";
            testUser.Metadata = "testData";

            Assert.AreEqual(1, testUser.Id);
            Assert.AreEqual("testName", testUser.Name);
            Assert.AreEqual("testData", testUser.Metadata);
        }

    }
}