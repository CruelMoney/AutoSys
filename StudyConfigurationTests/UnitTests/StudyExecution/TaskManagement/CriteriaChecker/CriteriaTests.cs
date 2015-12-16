#region Using

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StudyConfigurationServer.Logic.StudyExecution.TaskManagement.CriteriaValidation;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServerTests.UnitTests.StudyConfiguration.TaskManagement.CriteriaChecker
{
    [TestClass]
    public class CriteriaTests
    {
        [TestMethod]
        public void TestCriteriaValidatorCallsCheckerCorrectly()
        {
            //Arrange
            var criteria = new Criteria();
            var data = new string[1];
            var mockCriteriaChecker = new Mock<ICriteriaChecker>();
            var validator = new CriteriaValidator(
                new Dictionary<DataField.DataType, ICriteriaChecker>
                {
                    {It.IsAny<DataField.DataType>(), mockCriteriaChecker.Object}
                });

            mockCriteriaChecker.Setup(m => m.Validate(criteria, data)).Returns(true);

            //Action
            var result = validator.CriteriaIsMet(criteria, data);

            //Assert
            mockCriteriaChecker.Verify(m => m.Validate(criteria, data), Times.Once);
            Assert.IsTrue(result);
        }
    }
}