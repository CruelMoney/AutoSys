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
    public class StageTests
    {
        Stage testStage;
        StudyTask completeTask;
        StudyTask incompleteTask;
        StudyTask incompleteTask2;
        StudyTask incompleteTask3;
        StudyTask incompleteTask4;



        [TestInitialize]
        public void Initialize()
        {
            testStage = new Stage() {Tasks = new List<StudyTask>()};
            var user1 = new User() {Id = 1, Name = "user1"};
            var user2 = new User() {Id = 2, Name = "user2"};
            var userData1 = new UserData() { Data = new string[] {"done"}, UserID = 1 };
            var userData2 = new UserData() { Data = new string[] { }, UserID = 2};
            var completeDataField = new DataField() {Name = "testField", UserData = new List<UserData>() {userData1}};
            var incompletedataField = new DataField() { Name = "testField", UserData = new List<UserData>() { userData2 } }; ;

            completeTask = new StudyTask()
            {
                DataFields = new List<DataField>() { completeDataField }
            };

            incompleteTask = new StudyTask()
            {
                DataFields = new List<DataField>() { incompletedataField }
            };

            incompleteTask2 = new StudyTask()
            {
                DataFields = new List<DataField>() { incompletedataField, completeDataField }
            };

            incompleteTask3 = new StudyTask()
            {
                DataFields = new List<DataField>() { completeDataField, incompletedataField }
            };
            incompleteTask4 = new StudyTask()
            {
                DataFields = new List<DataField>() { completeDataField, incompletedataField, completeDataField }
            };
        }

        [TestMethod]
        public void TestStageFinished()
        {
            //Arrange
            testStage.Tasks.Add(completeTask);

            //Assert
            Assert.IsTrue(testStage.IsFinished());
        }

        [TestMethod]
        public void TestStageNotFinished()
        {
            //Arrange
            testStage.Tasks.AddRange(new List<StudyTask>() { incompleteTask });

            //Assert
            Assert.IsFalse(testStage.IsFinished());
            
            //Arrange
            testStage.Tasks.AddRange(new List<StudyTask>() { incompleteTask2 });

            //Assert
            Assert.IsFalse(testStage.IsFinished());
            //Arrange
            testStage.Tasks.AddRange(new List<StudyTask>() { incompleteTask3 });

            //Assert
            Assert.IsFalse(testStage.IsFinished());
            //Arrange
            testStage.Tasks.AddRange(new List<StudyTask>() { incompleteTask4 });

            //Assert
            Assert.IsFalse(testStage.IsFinished());
          
        }

        [TestMethod]
        public void TestStageNotFinishedMultipleTasks()
        {
            //Arrange
            testStage.Tasks.AddRange(new List<StudyTask>() { incompleteTask4, completeTask });

            //Assert
            Assert.IsFalse(testStage.IsFinished());

        }
    }
}
