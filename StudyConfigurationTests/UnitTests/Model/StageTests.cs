#region Using

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServerTests.UnitTests.Model
{
    [TestClass]
    public class StageTests
    {
        private StudyTask _completeTask;
        private StudyTask _incompleteTask;
        private StudyTask _incompleteTask2;
        private StudyTask _incompleteTask3;
        private StudyTask _incompleteTask4;
        private Stage _testStage;


        [TestInitialize]
        public void Initialize()
        {
            _testStage = new Stage {Tasks = new List<StudyTask>()};
            var user1 = new User {ID = 1, Name = "user1"};
            var user2 = new User {ID = 2, Name = "user2"};
            var userData1 = new UserData {Data = new List<StoredString> {new StoredString {Value = "done"}}, UserId = 1};
            var userData2 = new UserData {Data = new List<StoredString> {new StoredString()}, UserId = 2};
            var completeDataField = new DataField {Name = "testField", UserData = new List<UserData> {userData1}};
            var incompletedataField = new DataField {Name = "testField", UserData = new List<UserData> {userData2}};
            ;

            _completeTask = new StudyTask
            {
                DataFields = new List<DataField> {completeDataField}
            };

            _incompleteTask = new StudyTask
            {
                DataFields = new List<DataField> {incompletedataField}
            };

            _incompleteTask2 = new StudyTask
            {
                DataFields = new List<DataField> {incompletedataField, completeDataField}
            };

            _incompleteTask3 = new StudyTask
            {
                DataFields = new List<DataField> {completeDataField, incompletedataField}
            };
            _incompleteTask4 = new StudyTask
            {
                DataFields = new List<DataField> {completeDataField, incompletedataField, completeDataField}
            };
        }

        /*
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
            testStage.Tasks.Add( incompleteTask );

            //Assert
            Assert.IsFalse(testStage.IsFinished());
            
            //Arrange
            testStage.Tasks.Add( incompleteTask2 );

            //Assert
            Assert.IsFalse(testStage.IsFinished());
            //Arrange
            testStage.Tasks.Add( incompleteTask3 );

            //Assert
            Assert.IsFalse(testStage.IsFinished());
            //Arrange
            testStage.Tasks.Add( incompleteTask4 );

            //Assert
            Assert.IsFalse(testStage.IsFinished());
          
        }

        [TestMethod]
        public void TestStageNotFinishedMultipleTasks()
        {
            //Arrange
            testStage.Tasks.Add( incompleteTask4);
            testStage.Tasks.Add(completeTask);

            //Assert
            Assert.IsFalse(testStage.IsFinished());

        }
        */
    }
}