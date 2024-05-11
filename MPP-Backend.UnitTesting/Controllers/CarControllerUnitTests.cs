using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MPP_Backend.Business.Mappings;
using MPP_Backend.Business.Models;
using MPP_Backend.Business.Services;
using MPP_Backend.Controllers;
using MPP_Backend.Data.Models;
using MPP_Backend.Data.Repositories;

namespace MPP_Backend.UnitTesting.Controllers
{
    [TestClass]
    public class CarControllerUnitTests
    {
        private static CarManagerContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<CarManagerContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            return new CarManagerContext(options);
        }

        private static CarController CreateCarController(CarManagerContext context)
        {
            var carRepository = new CarRepository(context);

            var carMappingConfiguration = new MapperConfiguration(options => options.AddProfile(new CarMappingProfile()));
            var ownerMappingConfiguration = new MapperConfiguration(options => options.AddProfile(new OwnerMappingProfile()));
            var carMapper = new Mapper(carMappingConfiguration);
            var userMapper = new Mapper(ownerMappingConfiguration);

            var ownerRepository = new OwnerRepository(context);
            var ownerService = new OwnerService(ownerRepository, userMapper);

            var carService = new CarService(carRepository, ownerService, carMapper);

            return new CarController(carService, carMapper);
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
        public void TestAddCar_SuccessfulAddition_ReturnsCreatedAtRoute()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var carController = CreateCarController(context);

            var testCar = CreateTestCarForAddUpdateModel();

            // Act
            var addResult = carController.AddCar(testCar).Result;
            var createdAtRouteResult = (CreatedAtRouteResult)addResult;
            int id = Convert.ToInt32(createdAtRouteResult.RouteValues?["carId"]);

            var result = carController.GetCar(id).Result;
            var okResult = (OkObjectResult)result;
            var resultCar = (CarModel?)okResult.Value;

            // Assert
            Assert.IsInstanceOfType(addResult, typeof(CreatedAtRouteResult));

            Assert.IsNotNull(resultCar);
            Assert.AreEqual(id, resultCar.Id);
            Assert.AreEqual("test make", resultCar.Make);
            Assert.AreEqual(1, context.Cars.ToList().Count);
        }

        [TestMethod]
        public void TestDeleteCar_SuccessfulDeletion_ReturnsNoContent()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var carController = CreateCarController(context);

            var testCar = CreateTestCarForAddUpdateModel();
            var actionResult = carController.AddCar(testCar).Result;
            var createdAtRouteResult = (CreatedAtRouteResult)actionResult;
            int id = Convert.ToInt32(createdAtRouteResult.RouteValues?["carId"]);

            // Act
            var deleteResult = carController.DeleteCar(id).Result;
            var result = carController.GetCar(id).Result;

            // Assert
            Assert.IsInstanceOfType(deleteResult, typeof(NoContentResult));

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            Assert.AreEqual(0, context.Cars.ToList().Count);
        }

        [TestMethod]
        public void TestDeleteCar_InexistentCar_ReturnsNotFound()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var carController = CreateCarController(context);

            var testCar = CreateTestCarForAddUpdateModel();
            var actionResult = carController.AddCar(testCar).Result;
            var createdAtRouteResult = (CreatedAtRouteResult)actionResult;
            int id = Convert.ToInt32(createdAtRouteResult.RouteValues?["carId"]);

            // Act
            var deleteResult = carController.DeleteCar(-1).Result;

            var result = carController.GetCar(id).Result;
            var okResult = (OkObjectResult)result;
            var resultCar = (CarModel?)okResult.Value;

            // Assert
            Assert.IsInstanceOfType(deleteResult, typeof(NotFoundResult));

            Assert.IsNotNull(resultCar);
            Assert.AreEqual(id, resultCar.Id);
            Assert.AreEqual("test make", resultCar.Make);
            Assert.AreEqual(1, context.Cars.ToList().Count);
        }

        [TestMethod]
        public void TestUpdateCar_SuccessfulUpdate_ReturnsNoContent()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var carController = CreateCarController(context);

            var testCar = CreateTestCarForAddUpdateModel();
            var actionResult = carController.AddCar(testCar).Result;
            var createdAtRouteResult = (CreatedAtRouteResult)actionResult;
            int id = Convert.ToInt32(createdAtRouteResult.RouteValues?["carId"]);

            CarForAddUpdateModel replacementCar = new()
            {
                Make = "replacement make",
                Model = "replacement model",
                Colour = "replacement colour",
                ImageUrl = "replacement imageUrl"
            };

            // Act
            var updateResult = carController.UpdateCar(id, replacementCar).Result;

            var result = carController.GetCar(id).Result;
            var okResult = (OkObjectResult)result;
            var resultCar = (CarModel?)okResult.Value;

            // Assert
            Assert.IsInstanceOfType(updateResult, typeof(NoContentResult));

            Assert.IsNotNull(resultCar);
            Assert.AreEqual(id, resultCar.Id);
            Assert.AreEqual("replacement make", resultCar.Make);
            Assert.AreEqual(1, context.Cars.ToList().Count);
        }

        [TestMethod]
        public void TestUpdateCar_InexistentCar_ReturnsNotFound()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var carController = CreateCarController(context);

            var testCar = CreateTestCarForAddUpdateModel();
            var actionResult = carController.AddCar(testCar).Result;
            var createdAtRouteResult = (CreatedAtRouteResult)actionResult;
            int id = Convert.ToInt32(createdAtRouteResult.RouteValues?["carId"]);

            CarForAddUpdateModel replacementCar = new()
            {
                Make = "replacement make",
                Model = "replacement model",
                Colour = "replacement colour",
                ImageUrl = "replacement imageUrl"
            };

            // Act
            var updateResult = carController.UpdateCar(-1, replacementCar).Result;

            var result = carController.GetCar(id).Result;
            var okResult = (OkObjectResult)result;
            var resultCar = (CarModel?)okResult.Value;

            // Assert
            Assert.IsInstanceOfType(updateResult, typeof(NotFoundResult));

            Assert.IsNotNull(resultCar);
            Assert.AreEqual(id, resultCar.Id);
            Assert.AreEqual(testCar.Make, resultCar.Make);
            Assert.AreEqual(1, context.Cars.ToList().Count);
        }

        [TestMethod]
        public void TestGetCarById_Successful_ReturnsOk()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var carController = CreateCarController(context);

            var testCar = CreateTestCarForAddUpdateModel();
            var actionResult = carController.AddCar(testCar).Result;
            var createdAtRouteResult = (CreatedAtRouteResult)actionResult;
            int id = Convert.ToInt32(createdAtRouteResult.RouteValues?["carId"]);

            // Act
            var result = carController.GetCar(id).Result;
            var okResult = (OkObjectResult)result;
            var resultCar = (CarModel?)okResult.Value;

            // Assert
            Assert.IsNotNull(resultCar);
            Assert.AreEqual(id, resultCar.Id);
            Assert.AreEqual("test make", resultCar.Make);
        }

        [TestMethod]
        public void TestGetCarById_InexistentId_ReturnsNotFound()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var carController = CreateCarController(context);

            var testCar = CreateTestCarForAddUpdateModel();
            _ = carController.AddCar(testCar).Result;

            // Act
            var result = carController.GetCar(-1).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void TestGetAllCars()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var carController = CreateCarController(context);

            var testCar1 = CreateTestCarForAddUpdateModel();
            var testCar2 = CreateTestCarForAddUpdateModel();
            var testCar3 = CreateTestCarForAddUpdateModel();

            var actionResult = carController.AddCar(testCar1).Result;
            var createdAtRouteResult = (CreatedAtRouteResult)actionResult;
            int id1 = Convert.ToInt32(createdAtRouteResult.RouteValues?["carId"]);

            actionResult = carController.AddCar(testCar2).Result;
            createdAtRouteResult = (CreatedAtRouteResult)actionResult;
            int id2 = Convert.ToInt32(createdAtRouteResult.RouteValues?["carId"]);

            actionResult = carController.AddCar(testCar3).Result;
            createdAtRouteResult = (CreatedAtRouteResult)actionResult;
            int id3 = Convert.ToInt32(createdAtRouteResult.RouteValues?["carId"]);

            // Act
            var result = carController.GetAllCars().Result;

            var okResult = (OkObjectResult)result;
            var cars = ((IEnumerable<CarModel>?)okResult.Value)?.ToList();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            Assert.IsNotNull(cars);
            Assert.AreEqual(3, cars.Count);
            Assert.AreEqual(id1, cars[0].Id);
            Assert.AreEqual(id2, cars[1].Id);
            Assert.AreEqual(id3, cars[2].Id);
        }
    }
}
