using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Logic.StudyOverview;
using StudyConfigurationServer.Models;
using System.Collections.Generic;

namespace LogicTests1.StudyOverviewTests
{
    [TestClass]
    public class StudyOverviewTests
    {
        StudyOverviewController controller;
        Study study1, study2;
        List<UserStudies> userStudieList1, userStudieList2;
        List<Stage> stageList1;
        List<Stage> stageList2;
        List<StudyTask> taskList1;
        List<StudyTask> taskList2;
        List<StudyTask> taskList3;
        List<TaskRequestedData> listReqData1, listReqData2, listReqData3;
        TaskRequestedData reqData1, reqData2, reqData3;
        User user1, user2, user3;
        UserStudies userStudy1, userStudy2, userStudy3;
        StudyTask task1, task2, task3;
        Stage stage1, stage2, stage3;




        /*
        [TestInitialize]
        public void InitializeTests()
        {
            user1 = new User() { Name = "User1", Id = 1};
            user2 = new User() { Name = "User2", Id = 2};
            user3 = new User() { Name = "User3", Id = 3};

            userStudy1 = new UserStudies() { User = user1, Stage = stage1 };
            userStudy2 = new UserStudies() { User = user2, Stage = stage1 };
            userStudy3 = new UserStudies() { User = user3, Stage = stage2 };

            userStudieList1 = new List<UserStudies>() { userStudy1, userStudy2 };
            userStudieList2 = new List<UserStudies>() { userStudy3 };

            task1 = new StudyTask() { IsFinished = true, Id = 1 };
            task2 = new StudyTask() { IsFinished = false, Id = 2 };
            task3 = new StudyTask() { IsFinished = true, Id = 3 };

            taskList1 = new List<StudyTask>() { task1, task2, task3 };
            taskList2 = new List<StudyTask>() { task1, task2 };
            taskList3 = new List<StudyTask>() { task2, task3 };

            reqData1 = new TaskRequestedData() { User = user1, StudyTask = task1 };
            reqData2 = new TaskRequestedData() { User = user2, StudyTask = task2 };
            reqData3 = new TaskRequestedData() { User = user3, StudyTask = task3 };

            listReqData1 = new List<TaskRequestedData>() { reqData1 };
            listReqData2 = new List<TaskRequestedData>() { reqData2 };
            listReqData3 = new List<TaskRequestedData>() { reqData3 };

           
            stage1 = new Stage() { Id = 1, Study = study1, Tasks = taskList1, Users = userStudieList1 };
            stage2 = new Stage() { Id = 2, Study = study1, Tasks = taskList2, Users = userStudieList1 };
            stage3 = new Stage() { Id = 3, Study = study2, Tasks = taskList3, Users = userStudieList2 };

            stageList1 = new List<Stage>() { stage1, stage2 };
            stageList2 = new List<Stage>() { stage3 };

            study1 = new Study() { Name = "TestStudy", Id = 1, IsFinished = false, Stages = stageList1, CurrentStage = 1 };
            study2 = new Study() { Name = "TestStudy2", Id = 2, IsFinished = false, Stages = stageList2, CurrentStage = 1 };

            controller = new StudyOverviewController();
        }*/
        /*
        [TestMethod]
        public void TestRetrieveAllUserIdsFromStudy()
        {
            Assert.AreEqual(2, controller.GetUserIDs(study1).Length);
            Assert.AreEqual(1, controller.GetUserIDs(study2).Length);
            
            int id = controller.GetUserIDs(study1)[1];
            Assert.AreEqual(id, userStudy2.User.Id);
            
            Assert.AreEqual(controller.GetUserIDs(study2)[0], userStudy3.User.Id);
            
        }*/
        /*
        [TestMethod]
        public void Test()
        {

        }*/
    }
}
