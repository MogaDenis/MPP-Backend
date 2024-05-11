using Microsoft.EntityFrameworkCore;
using MPP_Backend.Data.Models;
using MPP_Backend.Data.Repositories;

namespace MPP_Backend.UnitTesting.Repositories
{
    [TestClass]
    public class CarRepositoryUnitTests
    {
        private static CarManagerContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<CarManagerContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var testOwner = new Owner()
            {
                FirstName = "Test",
                LastName = "Test"
            };

            CarManagerContext context = new (options);
            context.Owners.Add(testOwner);

            return context;
        }

        private static Car CreateTestCar()
        {
            return new Car()
            {
                Make = "test make",
                Model = "test model",
                Colour = "test colour",
                ImageUrl = "test imageUrl",
                OwnerId = 1,
            };
        }

        [TestMethod]
        public void TestAddCar_SuccessfulAddition()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var carRepository = new CarRepository(context);

            var testCar = CreateTestCar();

            // Act
            int id = carRepository.AddCarAsync(testCar).Result;
            var result = carRepository.GetCarByIdAsync(id).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual("test make", result.Make);
            Assert.AreEqual(1, context.Cars.ToList().Count);
        }

        [TestMethod]
        public void TestDeleteCar_SuccessfulDeletion_ReturnsTrue()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var carRepository = new CarRepository(context);

            var testCar = CreateTestCar();
            int id = carRepository.AddCarAsync(testCar).Result;

            // Act
            bool deleted = carRepository.DeleteCarAsync(id).Result; 
            var result = carRepository.GetCarByIdAsync(id).Result;

            // Assert
            Assert.IsTrue(deleted);
            Assert.IsNull(result);
            Assert.AreEqual(0, context.Cars.ToList().Count);
        }

        [TestMethod]
        public void TestDeleteCar_InexistentCar_ReturnsFalse()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var carRepository = new CarRepository(context);

            var testCar = CreateTestCar();
            int id = carRepository.AddCarAsync(testCar).Result;

            // Act
            bool deleted = carRepository.DeleteCarAsync(-1).Result;
            var result = carRepository.GetCarByIdAsync(id).Result;

            // Assert
            Assert.IsFalse(deleted);
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual("test make", result.Make);
            Assert.AreEqual(1, context.Cars.ToList().Count);
        }

        [TestMethod]
        public void TestUpdateCar_SuccessfulUpdate_ReturnsTrue()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var carRepository = new CarRepository(context);

            var testCar = CreateTestCar();
            int id = carRepository.AddCarAsync(testCar).Result;

            Car replacementCar = new()
            {
                Make = "replacement make",
                Model = "replacement model",
                Colour = "replacement colour",
                ImageUrl = "replacement imageUrl",
                OwnerId = 1
            };

            // Act
            bool updated = carRepository.UpdateCarAsync(id, replacementCar).Result;
            var result = carRepository.GetCarByIdAsync(id).Result;

            // Assert
            Assert.IsTrue(updated);
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual("replacement make", result.Make);
            Assert.AreEqual(1, context.Cars.ToList().Count);
        }

        [TestMethod]
        public void TestUpdateCar_InexistentCar_ReturnsFalse()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var carRepository = new CarRepository(context);

            var testCar = CreateTestCar();
            int id = carRepository.AddCarAsync(testCar).Result;

            Car replacementCar = new()
            {
                Make = "replacement make",
                Model = "replacement model",
                Colour = "replacement colour",
                ImageUrl = "replacement imageUrl"
            };

            // Act
            bool updated = carRepository.UpdateCarAsync(-1, replacementCar).Result;
            var result = carRepository.GetCarByIdAsync(id).Result;

            // Assert
            Assert.IsFalse(updated);
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(testCar.Make, result.Make);
            Assert.AreEqual(1, context.Cars.ToList().Count);
        }

        [TestMethod]
        public void TestGetCarById_Successful()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var carRepository = new CarRepository(context);

            var testCar = CreateTestCar();      
            int id = carRepository.AddCarAsync(testCar).Result;

            // Act
            var result = carRepository.GetCarByIdAsync(id).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual("test make", result.Make);
        }

        [TestMethod]
        public void TestGetCarById_InexistentId_ReturnsNull() 
        {
            // Arrange
            var context = CreateInMemoryContext();
            var carRepository = new CarRepository(context);

            var testCar = CreateTestCar();
            _ = carRepository.AddCarAsync(testCar).Result;

            // Act
            var result = carRepository.GetCarByIdAsync(-1).Result;

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestGetAllCars()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var carRepository = new CarRepository(context);

            var testCar1 = CreateTestCar();
            var testCar2 = CreateTestCar();
            var testCar3 = CreateTestCar();

            int id1 = carRepository.AddCarAsync(testCar1).Result;
            int id2 = carRepository.AddCarAsync(testCar2).Result;
            int id3 = carRepository.AddCarAsync(testCar3).Result;

            // Act
            var result = carRepository.GetAllCarsAsync().Result.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(id1, result[0].Id);
            Assert.AreEqual(id2, result[1].Id);
            Assert.AreEqual(id3, result[2].Id);
        }
    }
}