using Microsoft.EntityFrameworkCore;
using MPP_Backend.Data.Models;
using MPP_Backend.Data.Repositories;

namespace MPP_Backend.UnitTesting.Repositories
{
    [TestClass]
    public class OwnerRepositoryUnitTests
    {
        private static CarManagerContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<CarManagerContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            return new CarManagerContext(options);
        }

        private static Owner CreateTestOwner()
        {
            return new Owner()
            {
                FirstName = "test",
                LastName = "test"
            };
        }

        [TestMethod]
        public async Task TestAddUser_SuccessfulAddition()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var ownerRepository = new OwnerRepository(context);

            var testOwner = CreateTestOwner();

            // Act
            int id = await ownerRepository.AddOwnerAsync(testOwner);

            var resultOwner = ownerRepository.GetOwnerByIdAsync(id).Result;

            // Assert
            Assert.IsNotNull(resultOwner);
            Assert.AreEqual(testOwner.FirstName, resultOwner.FirstName);
            Assert.AreEqual(1, context.Owners.ToList().Count);
        }

        [TestMethod]
        public async Task TestGetUser_Successful()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var ownerRepository = new OwnerRepository(context);

            var testOwner = CreateTestOwner();
            int id = await ownerRepository.AddOwnerAsync(testOwner);

            // Act
            var resultOwner = ownerRepository.GetOwnerByIdAsync(id).Result;

            // Assert
            Assert.IsNotNull(resultOwner);
            Assert.AreEqual(testOwner.FirstName, resultOwner.FirstName);
            Assert.AreEqual(1, context.Owners.ToList().Count);
        }

        [TestMethod]
        public async Task TestGetUser_InexistentUsername_ReturnsNull()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var ownerRepository = new OwnerRepository(context);

            var testOwner = CreateTestOwner();
            await ownerRepository.AddOwnerAsync(testOwner);

            // Act
            var resultOwner = ownerRepository.GetOwnerByIdAsync(-1).Result;

            // Assert
            Assert.IsNull(resultOwner);
        }
    }
}
