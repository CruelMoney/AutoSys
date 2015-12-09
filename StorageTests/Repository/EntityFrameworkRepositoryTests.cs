﻿using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Storage.Repository;

namespace StorageTests.Repository
{
    [TestClass]
    public class EntityFrameworkGenericRepositoryTests
    {

        private Mock defaultDBContext()
        {
            return new Mock<MockContext>();
        }


        [TestMethod]
        public void EFRepoDefaultConstructorRunsTest()
        {
            //Act
            var efDatabase = new EntityFrameworkGenericRepository<MockContext>();

            //Assert
            Assert.IsNotNull(efDatabase);
        }

        [TestMethod]
        public void EFRepoSecondConstructorRunsTest()
        {
            var efDatabase = new EntityFrameworkGenericRepository<MockContext>((MockContext)defaultDBContext().Object);

            //Assert
            Assert.IsNotNull(efDatabase);
        }

        

        [TestMethod]
        public void EFRepoCreateTest()
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
        public void EFRepoDeleteTest()
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
        public void EFRepoReadTest()
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
        public void EFRepoReadItemTest()
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

        [TestMethod]
        public void EFRepoUpdateItemTest()
        {
            var DbSet = new Mock<DbSet<MockEntity>>();
            var context = new Mock<MockContext>();
            var EFRepo = new EntityFrameworkGenericRepository<MockContext>(context.Object);
            var item = new MockEntity() { Id = 1, Name = "OldName" };

            DbSet.Setup(m => m.Find(item.Id)).Returns(item);
            context.Setup(c => c.Set<MockEntity>()).Returns(DbSet.Object);
           
            EFRepo.Update(item);

            context.Verify(c => c.SaveChangesAsync(), Times.Once);
            DbSet.Verify(m => m.Attach(item), Times.Once);
            
        }

    }
}