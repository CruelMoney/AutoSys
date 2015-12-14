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
            var user1 = new User() {ID = 1 };
            var user2 = new User() { ID = 2 };
            var user3 = new User() { ID = 2 };

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
                Users = new List<User>()
            };
            var task2 = new StudyTask()
            {
                DataFields = new List<DataField>() { dataField3 },
                Users = new List<User>()
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
                    Assert.AreEqual(users[0].ID, dataField.UserData[0].UserID);
                    Assert.AreEqual(users[1].ID, dataField.UserData[1].UserID);
                    Assert.AreEqual(users[2].ID, dataField.UserData[2].UserID);
                }
                Assert.AreEqual(users[0], task.Users[0]);
                Assert.AreEqual(users[1], task.Users[1]);
                Assert.AreEqual(users[2], task.Users[2]);
            }
           
        }

       
    }
}
