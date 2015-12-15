#region Using

using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Storage.Repository;

#endregion

namespace StorageTests.Repository
{
    [TestClass]
    public class EntityFrameworkGenericRepositoryTests
    {
        private Mock DefaultDbContext()
        {
            return new Mock<MockContext>();
        }


        [TestMethod]
        public void TestEfRepoDefaultConstructorRuns()
        {
            //Act
            var efDatabase = new EntityFrameworkGenericRepository<MockContext>();

            //Assert
            Assert.IsNotNull(efDatabase);
        }

        [TestMethod]
        public void TestEfRepoSecondConstructorRuns()
        {
            var efDatabase = new EntityFrameworkGenericRepository<MockContext>((MockContext) DefaultDbContext().Object);

            //Assert
            Assert.IsNotNull(efDatabase);
        }


        [TestMethod]
        public void TestEfRepoCreate()
        {
            //Arrange
            var dbset = new Mock<DbSet<MockEntity>>();
            var context = new Mock<MockContext>();
            var efRepo = new EntityFrameworkGenericRepository<MockContext>(context.Object);
            var expectedItem = new MockEntity();

            context.Setup(c => c.Set<MockEntity>()).Returns(dbset.Object);

            efRepo.Create(expectedItem);

            context.Verify(c => c.SaveChanges(), Times.Once);
            dbset.Verify(m => m.Add(expectedItem), Times.Once);
        }

        [TestMethod]
        public void TestEfRepoDelete()
        {
            //Arrange
            var dbset = new Mock<DbSet<MockEntity>>();
            var context = new Mock<MockContext>();
            var efRepo = new EntityFrameworkGenericRepository<MockContext>(context.Object);
            var expectedItem = new MockEntity();

            context.Setup(c => c.Set<MockEntity>()).Returns(dbset.Object);

            efRepo.Delete(expectedItem);

            context.Verify(c => c.SaveChanges(), Times.Once);
            dbset.Verify(m => m.Remove(expectedItem), Times.Once);
        }

        [TestMethod]
        public void TestEfRepoRead()
        {
            //Arrange
            var expectedDbset = new Mock<DbSet<MockEntity>>();
            var context = new Mock<MockContext>();
            var efRepo = new EntityFrameworkGenericRepository<MockContext>(context.Object);
            var expectedItem = new MockEntity();

            context.Setup(c => c.Set<MockEntity>()).Returns(expectedDbset.Object);

            //Action
            var actual = efRepo.Read<MockEntity>();

            //Assert
            Assert.AreEqual(expectedDbset.Object, actual);
        }

        [TestMethod]
        public void TestEfRepoReadItem()
        {
            //Arrange
            var expectedItem = new MockEntity {ID = 1};
            var expectedDbset = new Mock<DbSet<MockEntity>>();
            var context = new Mock<MockContext>();
            var efRepo = new EntityFrameworkGenericRepository<MockContext>(context.Object);

            expectedDbset.Setup(m => m.Find(expectedItem.ID)).Returns(expectedItem);
            context.Setup(c => c.Set<MockEntity>()).Returns(expectedDbset.Object);

            //Action
            var actual = efRepo.Read<MockEntity>(expectedItem.ID);

            //Assert
            Assert.AreEqual(expectedItem, actual);
        }
    }
}