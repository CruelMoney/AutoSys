using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Logic.StudyConfiguration.TaskManagement.TaskDistributor;
using StudyConfigurationServer.Models;

namespace LogicTests1.StudyConfiguration.TaskManagement.TaskDistributor
{
    [TestClass]
    public class DistributorsTests
    {
        List<User> users;
        List<StudyTask> tasks;
        
        [TestInitialize]
        public void Initialize()
        {
            var user1 = new User() {Id = 1 };
            var user2 = new User() { Id = 2 };
            var user3 = new User() { Id = 2 };

            users = new List<User>() {user1,user2,user3};

            var dataField1 = new DataField()
            {
                UserData = new List<UserData>()
            };
            var dataField2 = new DataField()
            {
                UserData = new List<UserData>()
            };
            var dataField3 = new DataField()
            {
                UserData = new List<UserData>()
            };

            var task1 = new StudyTask()
            {
                DataFields = new List<DataField>() { dataField1, dataField2 },
                UserIDs = new List<int>()
            };
            var task2 = new StudyTask()
            {
                DataFields = new List<DataField>() { dataField3 },
                UserIDs = new List<int>()
            };

            tasks = new List<StudyTask>() {task1, task2};
            
        }

        [TestMethod]
        public void TestDistributorsEqual()
        {
            //Arrange
            var distributor = new EqualDistributor();

            //Action
            var result = distributor.Distribute(users, tasks).ToList();

            //Assert
            Assert.AreEqual(2, result.Count);

            foreach (var task in result)
            {

                foreach (var dataField in task.DataFields)
                {
                    Assert.AreEqual(users[0].Id, dataField.UserData[0].UserID);
                    Assert.AreEqual(users[1].Id, dataField.UserData[1].UserID);
                    Assert.AreEqual(users[2].Id, dataField.UserData[2].UserID);
                }
                Assert.AreEqual(users[0].Id, task.UserIDs[0]);
                Assert.AreEqual(users[1].Id, task.UserIDs[1]);
                Assert.AreEqual(users[2].Id, task.UserIDs[2]);
            }
           
        }

       
    }
}
