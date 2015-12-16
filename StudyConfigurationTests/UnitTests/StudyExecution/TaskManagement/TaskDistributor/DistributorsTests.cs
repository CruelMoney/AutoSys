#region Using

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Logic.StudyExecution.TaskManagement.TaskDistributor;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServerTests.UnitTests.StudyConfiguration.TaskManagement.TaskDistributor
{
    [TestClass]
    public class DistributorsTests
    {
        private List<StudyTask> _tasks;
        private List<User> _users;
        private List<User> _users2;

        [TestInitialize]
        public void Initialize()
        {
            var user1 = new User {ID = 1};
            var user2 = new User {ID = 2};
            var user3 = new User {ID = 2};

            _users = new List<User> {user1, user2, user3};
            _users2 = new List<User> {user1};

            var dataField1 = new DataField
            {
                UserData = new List<UserData>()
            };
            var dataField2 = new DataField
            {
                UserData = new List<UserData>()
            };
            var dataField3 = new DataField
            {
                UserData = new List<UserData>()
            };

            var task1 = new StudyTask
            {
                DataFields = new List<DataField> {dataField1, dataField2},
                Users = new List<User>()
            };
            var task2 = new StudyTask
            {
                DataFields = new List<DataField> {dataField3},
                Users = new List<User>()
            };

            _tasks = new List<StudyTask> {task1, task2};
        }

        [TestMethod]
        public void TestDistributorsEqual()
        {
            //Arrange
            var distributor = new EqualDistributor();

            //Action
            var result = distributor.Distribute(_users, _tasks).ToList();

            //Assert
            Assert.AreEqual(2, result.Count);

            foreach (var task in result)
            {
                foreach (var dataField in task.DataFields)
                {
                    Assert.AreEqual(_users[0].ID, dataField.UserData[0].UserId);
                    Assert.AreEqual(_users[1].ID, dataField.UserData[1].UserId);
                    Assert.AreEqual(_users[2].ID, dataField.UserData[2].UserId);
                }
                Assert.AreEqual(_users[0], task.Users[0]);
                Assert.AreEqual(_users[1], task.Users[1]);
                Assert.AreEqual(_users[2], task.Users[2]);
            }
        }

        [TestMethod]
        public void TestDistributorsEqualOnePerson()
        {
            //Arrange
            var distributor = new EqualDistributor();

            //Action
            var result = distributor.Distribute(_users2, _tasks).ToList();

            //Assert
            Assert.AreEqual(2, result.Count);

            foreach (var task in result)
            {
                Assert.AreEqual(1, task.Users.Count);

                foreach (var dataField in task.DataFields)
                {
                    Assert.AreEqual(_users2[0].ID, dataField.UserData[0].UserId);
                    Assert.AreEqual(1, dataField.UserData.Count);
                }
                Assert.AreEqual(_users[0], task.Users[0]);
            }
        }


        [TestMethod]
        public void TestDistributorsNoOverlap()
        {
            //Arrange
            var distributor = new NoOverlapDistributor();

            //Action
            var result = distributor.Distribute(_users, _tasks).ToList();

            //Assert
            Assert.AreEqual(2, result.Count);

            foreach (var task in result)
            {
                Assert.AreEqual(1, task.Users.Count);

                foreach (var dataField in task.DataFields)
                {
                    Assert.AreEqual(1, dataField.UserData.Count);
                }
            }
        }
    }
}