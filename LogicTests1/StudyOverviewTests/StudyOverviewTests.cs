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
        Study study, study2;
        List<UserStudies> studieList1, studieList2, studieList3;
        List<Stage> stages;
        List<StudyTask> tasks;
        List<TaskRequestedData> reqData1, reqData2, reqData3;
        User user1, user2, user3;
        UserStudies study1, studie2, studie3;
         
        


        [TestInitialize]
        public void InitializeTests()
        {
            user1 = new User() {Name = "User1", Id= 1, Studies = studieList1, Tasks = reqData1 };
            user2 = new User() { Name = "User2", Id = 2, Studies = studieList2, Tasks = reqData2 };
            user3 = new User() { Name = "User3", Id = 3, Studies = studieList3, Tasks = reqData3 };

            controller = new StudyOverviewController();
            studies = new List<UserStudies>() { new UserStudies() { User = user }, new UserStudies() { User = user2 } };
            stages = new List<Stage>();
            reqData = new List<TaskRequestedData>();
            study = new Study() { Name = "TestStudy", Id = 1, IsFinished = false, Stages = stages, Users = studies, CurrentStage = 1};

        }

        [TestMethod]
        public void TestRetrieveAllUserIdsFromStudy()
        {
        }
    }
}
