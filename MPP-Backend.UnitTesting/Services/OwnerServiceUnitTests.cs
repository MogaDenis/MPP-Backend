﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MPP_Backend.Business.Mappings;
using MPP_Backend.Business.DTOs;
using MPP_Backend.Business.Services;
using MPP_Backend.Data.Models;
using MPP_Backend.Data.Repositories;

namespace MPP_Backend.UnitTesting.Services
{
    [TestClass]
    public class OwnerServiceUnitTests
    {
        private static CarManagerContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<CarManagerContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            return new CarManagerContext(options);
        }

        private static OwnerService CreateOwnerService(CarManagerContext context)
        {
            var ownerRepository = new OwnerRepository(context);
            var configuration = new MapperConfiguration(options =>
                options.AddProfile(new OwnerMappingProfile()));

            var mapper = new Mapper(configuration);

            return new OwnerService(ownerRepository, mapper);
        }

        private static OwnerForAddUpdateDTO CreateTestOwnerModel()
        {
            return new OwnerForAddUpdateDTO()
            {
                FirstName = "test",
                LastName = "test"
            };
        }

        [TestMethod]
        public async Task TestAddOwner_SuccessfulAddition_ReturnsId()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var ownerService = CreateOwnerService(context);

            var testOwnerModel = CreateTestOwnerModel();

            // Act
            var addedOwner = await ownerService.AddOwnerAsync(testOwnerModel);

            // Assert
            Assert.IsNotNull(addedOwner);
            Assert.AreEqual(1, context.Owners.ToList().Count);
        }
    }
}
