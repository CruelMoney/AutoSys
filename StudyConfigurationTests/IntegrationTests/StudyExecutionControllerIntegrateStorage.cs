using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storage.Repository;
using StudyConfigurationServer.Api;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Logic.StudyExecution;
using StudyConfigurationServer.Logic.StudyManagement;
using StudyConfigurationServer.Models.Data;
using StudyConfigurationServer.Models.DTO;
using StudyConfigurationServerTests.IntegrationTests.DBInitializers;
using StudyConfigurationServerTests.Properties;

namespace StudyConfigurationServerTests.IntegrationTests
{
    [TestClass]
    public class StudyExecutionControllerIntegrateStorage
    {
        private StudyExecutionController _manager;
        private EntityFrameworkGenericRepository<StudyContext> _repo;

        [TestInitialize]
        public void Initialize()
        {
            _manager = new StudyExecutionController();
        }

        private void InitializeStudyDb()
        {
            Database.SetInitializer(new StudyDb());
            var context = new StudyContext();
            context.Database.Initialize(true);
        }
     

        [TestMethod]
        public void DeliverTask()
        {
            //Arrange
            InitializeStudyDb();

            var taskDto = new TaskSubmissionDto
            {
                UserId = 1,
                SubmittedFields = new[]
                {
                    new DataFieldDto {Data = new[] {"updatedData"}, Name = "Year"}
                }
            };
            //Action
            _manager.DeliverTask(1, 1, taskDto);
            var result = _manager.GetTasks(1, 1, 1, TaskRequestDto.Filter.Editable, TaskRequestDto.Type.Review).ToList();
            var actualTask = result.First();

            //Assert
            Assert.AreEqual(1, actualTask.Id);
            Assert.AreEqual(true, actualTask.IsDeliverable);
            Assert.AreEqual("updatedData", actualTask.RequestedFields[0].Data[0]);
        }
        
    }
}

