using StudyConfigurationServer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicTests1.Model
{
    [TestClass]
    public class TaskRequestedDataTests
    {
        TaskRequestedData completedTask ;
        TaskRequestedData incompleteTask;
        TaskRequestedData incompleteTask2;
        TaskRequestedData incompleteTask3;


        [TestInitialize]
        public void Initialize()
        {
           
        }

        [TestMethod]
        public void TestUserTaskCompleted()
        {
            var completeData = new string[] { "testData" };
            var completeData2 = new string[] { "dataTest" };
            var nullData = new string[3];
            var emptyStringsData = new string[3] { "", "", "" };

            var completeField = new DataField()
            {
                Data = completeData,
            };
            var completeField2 = new DataField()
            {
                Data = completeData2,
            };
            var nullField = new DataField()
            {
                Data = nullData,
            };
            var emptyStringsField = new DataField()
            {
                Data = emptyStringsData
            };

            completedTask = new TaskRequestedData() { Data = { completeField, completeField2 }  };
            incompleteTask = new TaskRequestedData() { Data = { nullField, completeField2 } };
            incompleteTask2 = new TaskRequestedData() { Data = { completeField2, nullField } };
            incompleteTask3 = new TaskRequestedData() { Data = { completeField2, nullField, completeField } };
            Assert.IsTrue(completedTask.IsTaskFinished());
            Assert.IsFalse(incompleteTask.IsTaskFinished());
            Assert.IsFalse(incompleteTask2.IsTaskFinished());
            Assert.IsFalse(incompleteTask3.IsTaskFinished());
        }
    }
}
