using AutoMapper;
using BusinessServices.Services;
using BusinessServices.Services.Mappers;
using Entity.DTOs;
using Entity.Entity;
using Entity.Entity.Models;
using Entity.Models;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;

namespace LvrUnitTest
{
    public class LrvServiceTests
    {
        private readonly LvrService _service;
        private readonly LvrDbContext _fakeDbContext;
        private readonly ILvrMapper _fakeMapperConfig;
        private readonly IMapper _fakeMapper;

        public LrvServiceTests()
        {
            var options = new DbContextOptionsBuilder<LvrDbContext>()
                       .UseInMemoryDatabase(databaseName: "TestDatabase")
                       .Options;
            _fakeDbContext = new LvrDbContext(options);

            _fakeMapperConfig = A.Fake<ILvrMapper>();
            _fakeMapper = A.Fake<IMapper>();

            A.CallTo(() => _fakeMapperConfig.LvrConfiguration()).Returns(_fakeMapper);

            _service = new LvrService(_fakeDbContext, _fakeMapperConfig);
        }


        [Fact]
        public async Task CalculateLVRAsync_ShouldReturnErrorForInvalidInput()
        {
            // Arrange
            var input = new InputLVR { PropertyValue = 0, LoanAmount = 0 };

            // Act
            var result = await _service.CalculateLVRAsync(input);

            // Assert
            Assert.Equal("Invalid input data.", result.Message);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task CalculateLVRAsync_ShouldReturnErrorWhenLvrExists()
        {
            // Arrange
            var input = new InputLVR { PropertyValue = 500000, LoanAmount = 300000 };
            decimal lvr = (input.LoanAmount / input.PropertyValue) * 100;

            var existingLvr = new LVR { LoanAmount = input.LoanAmount, PropertyValue = input.PropertyValue, LoanValuationRatio = lvr };
            _fakeDbContext.LVR.Add(existingLvr);
            await _fakeDbContext.SaveChangesAsync();

            // Act
            var result = await _service.CalculateLVRAsync(input);

            // Assert
            Assert.Equal("This LVR already exists in the database.", result.Message);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task CalculateLVRAsync_ShouldSaveLvrAndReturnSuccess()
        {
            // Arrange
            var input = new InputLVR { PropertyValue = 400000, LoanAmount = 200000 };

            _fakeDbContext.LVR.RemoveRange(_fakeDbContext.LVR);

            var expectedDto = new LVRDto { Message = "Record saved successfully.", StatusCode = 201 };
            A.CallTo(() => _fakeMapper.Map<LVRDto>(A<LVR>._)).Returns(expectedDto);

            // Act
            var result = await _service.CalculateLVRAsync(input);

            // Assert
            Assert.Equal("Record saved successfully.", result.Message);
            Assert.Equal(201, result.StatusCode);
        }
    }
}