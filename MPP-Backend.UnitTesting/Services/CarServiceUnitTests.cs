using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MPP_Backend.Business.Mappings;
using MPP_Backend.Business.Models;
using MPP_Backend.Business.Services;
using MPP_Backend.Data.Models;
using MPP_Backend.Data.Repositories;

namespace MPP_Backend.UnitTesting.Services
{
    [TestClass]
    public class CarServiceUnitTests
    {
        private static CarManagerContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<CarManagerContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            return new CarManagerContext(options);
        }

        private static CarService CreateCarService(CarManagerContext context)
        {
            var carRepository = new CarRepository(context);

            var carMappingConfiguration = new MapperConfiguration(options => options.AddProfile(new CarMappingProfile()));
            var ownerMappingConfiguration = new MapperConfiguration(options => options.AddProfile(new OwnerMappingProfile()));
            var carMapper = new Mapper(carMappingConfiguration);
            var userMapper = new Mapper(ownerMappingConfiguration);

            var userRepository = new OwnerRepository(context);
            var ownerService = new OwnerService(userRepository, userMapper);

            ownerService.AddOwnerAsync(new OwnerModel() { FirstName = "Test", LastName = "Test" }).Wait();

            return new CarService(carRepository, ownerService, carMapper);
        }

        private static CarForAddUpdateModel CreateTestCarForAddUpdateModel()
        {
            return new CarForAddUpdateModel()
            {
                Make = "test make",
                Model = "test model",
                Colour = "test colour",
                ImageUrl = "test imageUrl"
            };
        }

        [TestMethod]
        public void TestAddCar_SuccessfulAddition()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var carService = CreateCarService(context);

            var testCar = CreateTestCarForAddUpdateModel();

            // Act
            int id = carService.AddCarAsync(testCar).Result;
            var result = carService.GetCarByIdAsync(id).Result;

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
            var carService = CreateCarService(context);

            var testCar = CreateTestCarForAddUpdateModel();
            int id = carService.AddCarAsync(testCar).Result;

            // Act
            bool deleted = carService.DeleteCarAsync(id).Result;
            var result = carService.GetCarByIdAsync(id).Result;

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
            var carService = CreateCarService(context);

            var testCar = CreateTestCarForAddUpdateModel();
            int id = carService.AddCarAsync(testCar).Result;

            // Act
            bool deleted = carService.DeleteCarAsync(-1).Result;
            var result = carService.GetCarByIdAsync(id).Result;

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
            var carService = CreateCarService(context);

            var testCar = CreateTestCarForAddUpdateModel();
            int id = carService.AddCarAsync(testCar).Result;

            CarForAddUpdateModel replacementCar = new()
            {
                Make = "replacement make",
                Model = "replacement model",
                Colour = "replacement colour",
                ImageUrl = "replacement imageUrl"
            };

            // Act
            bool updated = carService.UpdateCarAsync(id, replacementCar).Result;
            var result = carService.GetCarByIdAsync(id).Result;

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
            var carService = CreateCarService(context);

            var testCar = CreateTestCarForAddUpdateModel();
            int id = carService.AddCarAsync(testCar).Result;

            CarForAddUpdateModel replacementCar = new()
            {
                Make = "replacement make",
                Model = "replacement model",
                Colour = "replacement colour",
                ImageUrl = "replacement imageUrl"
            };

            // Act
            bool updated = carService.UpdateCarAsync(-1, replacementCar).Result;
            var result = carService.GetCarByIdAsync(id).Result;

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
            var carService = CreateCarService(context);

            var testCar = CreateTestCarForAddUpdateModel();
            int id = carService.AddCarAsync(testCar).Result;

            // Act
            var result = carService.GetCarByIdAsync(id).Result;

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
            var carService = CreateCarService(context);

            var testCar = CreateTestCarForAddUpdateModel();
            _ = carService.AddCarAsync(testCar).Result;

            // Act
            var result = carService.GetCarByIdAsync(-1).Result;

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestGetAllCars()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var carService = CreateCarService(context);

            var testCar1 = CreateTestCarForAddUpdateModel();
            var testCar2 = CreateTestCarForAddUpdateModel();
            var testCar3 = CreateTestCarForAddUpdateModel();

            int id1 = carService.AddCarAsync(testCar1).Result;
            int id2 = carService.AddCarAsync(testCar2).Result;
            int id3 = carService.AddCarAsync(testCar3).Result;

            // Act
            var result = carService.GetAllCarsAsync().Result.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(id1, result[0].Id);
            Assert.AreEqual(id2, result[1].Id);
            Assert.AreEqual(id3, result[2].Id);
        }

        [TestMethod]
        public void TestGetCarsOfUser()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var carService = CreateCarService(context);

            var testCar1 = CreateTestCarForAddUpdateModel();
            var testCar2 = CreateTestCarForAddUpdateModel();
            var testCar3 = CreateTestCarForAddUpdateModel();

            int ownerId = 1;

            testCar1.OwnerId = ownerId;
            testCar2.OwnerId = ownerId;

            int id1 = carService.AddCarAsync(testCar1).Result;
            int id2 = carService.AddCarAsync(testCar2).Result;
            int id3 = carService.AddCarAsync(testCar3).Result;

            // Act
            var result = carService.GetCarsOfOwnerAsync(ownerId).Result.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(id1, result[0].Id);
            Assert.AreEqual(id2, result[1].Id);
        }
    }
}