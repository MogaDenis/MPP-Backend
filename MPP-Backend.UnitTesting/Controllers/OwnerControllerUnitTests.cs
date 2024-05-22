using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MPP_Backend.Business.Mappings;
using MPP_Backend.Business.DTOs;
using MPP_Backend.Business.Services;
using MPP_Backend.Controllers;
using MPP_Backend.Data.Models;
using MPP_Backend.Data.Repositories;

namespace MPP_Backend.UnitTesting.Controllers
{
    [TestClass]
    public class OwnerControllerUnitTests
    {
        private static CarManagerContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<CarManagerContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            return new CarManagerContext(options);
        }

        private static OwnerController CreateOwnerController(CarManagerContext context)
        {
            var ownerRepository = new OwnerRepository(context);
            var configuration = new MapperConfiguration(options =>
                options.AddProfile(new OwnerMappingProfile()));

            var mapper = new Mapper(configuration);
            var ownerService = new OwnerService(ownerRepository, mapper);

            return new OwnerController(ownerService);
        }

        private static OwnerForAddUpdateDTO CreateTestUserModel()
        {
            return new OwnerForAddUpdateDTO()
            {

            };
        }

        [TestMethod]
        public async Task TestAddUser_SuccessfulAddition_ReturnsCreatedAtRouteResult()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var ownerController = CreateOwnerController(context);

            var testUserModel = CreateTestUserModel();

            // Act
            var addResult = await ownerController.AddOwner(testUserModel);

            // Assert
            Assert.IsInstanceOfType(addResult, typeof(CreatedAtRouteResult));
            Assert.AreEqual(1, context.Owners.ToList().Count);
        }
    }
}
