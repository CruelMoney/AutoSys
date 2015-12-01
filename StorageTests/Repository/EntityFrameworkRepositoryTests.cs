using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storage.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Logic.Model.DTO;
using Moq;
using StorageTests.Repository;

namespace Storage.Repository.Tests
{
    [TestClass]
    public class EntityFrameworkGenericRepositoryTests
    {

        private Mock defaultDBContext()
        {
            return new Mock<MockContext>();
        }


        [TestMethod]
        public void DefaultConstructor_Runs()
        {
            //Act
            var efDatabase = new EntityFrameworkGenericRepository<MockContext>();

            //Assert
            Assert.IsNotNull(efDatabase);
        }

        [TestMethod]
        public void SecondConstructor_Runs()
        {
            var efDatabase = new EntityFrameworkGenericRepository<MockContext>((MockContext)defaultDBContext().Object);

            //Assert
            Assert.IsNotNull(efDatabase);
        }

        

        [TestMethod]
        public void CreateTest()
        {
            //Arrange
            var dbset = new Mock<DbSet<MockEntity>>();
            var context = new Mock<MockContext>();
            var EFRepo = new EntityFrameworkGenericRepository<MockContext>(context.Object);
            var expectedItem = new MockEntity();

            context.Setup(c => c.Set<MockEntity>()).Returns(dbset.Object);

            EFRepo.Create(expectedItem);

            context.Verify(c => c.SaveChangesAsync(), Times.Once);
            dbset.Verify(m => m.Add(expectedItem), Times.Once);
        }

        [TestMethod]
        public void DeleteTest()
        {

            //Arrange
            var dbset = new Mock<DbSet<MockEntity>>();
            var context = new Mock<MockContext>();
            var EFRepo = new EntityFrameworkGenericRepository<MockContext>(context.Object);
            var expectedItem = new MockEntity();

            context.Setup(c => c.Set<MockEntity>()).Returns(dbset.Object);
            
            EFRepo.Delete(expectedItem);

            context.Verify(c => c.SaveChangesAsync(), Times.Once);
            dbset.Verify(m => m.Remove(expectedItem), Times.Once);
        }

        [TestMethod]
        public void ReadTest()
        {
            //Arrange
            var expectedDbset = new Mock<DbSet<MockEntity>>();
            var context = new Mock<MockContext>();
            var EFRepo = new EntityFrameworkGenericRepository<MockContext>(context.Object);
            var expectedItem = new MockEntity();

            context.Setup(c => c.Set<MockEntity>()).Returns(expectedDbset.Object);

            //Action
            var actual = EFRepo.Read<MockEntity>();

            //Assert
            Assert.AreEqual(expectedDbset.Object, actual);
        }

        [TestMethod]
        public void ReadItemTest()
        {
            //Arrange
            var expectedItem = new MockEntity() { Id = 1};
            var expectedDbset = new Mock<DbSet<MockEntity>>();
            var context = new Mock<MockContext>();
            var EFRepo = new EntityFrameworkGenericRepository<MockContext>(context.Object);

            expectedDbset.Setup(m => m.Find(expectedItem.Id)).Returns(expectedItem);
            context.Setup(c => c.Set<MockEntity>()).Returns(expectedDbset.Object);

            //Action
            var actual = EFRepo.Read<MockEntity>(expectedItem.Id);

            //Assert
            Assert.AreEqual(expectedItem, actual);
        }

    }
}