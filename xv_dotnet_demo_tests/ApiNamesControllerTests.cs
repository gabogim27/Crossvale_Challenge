using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xv_dotnet_demo.Controllers;
using xv_dotnet_demo.Dtos;
using xv_dotnet_demo_v2_domain.Entities;
using xv_dotnet_demo_v2_services;

namespace xv_dotnet_demo_tests
{
    public class ApiNamesControllerTests
    {
        private Mock<INamesService> _namesServiceMock;
        private Mock<IMapper> _mapperMock;
        private ApiNamesController _controller;

        [SetUp]
        public void Setup()
        {
            _namesServiceMock = new Mock<INamesService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new ApiNamesController(_namesServiceMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetName_ReturnsBadRequest_WhenIdIsInvalid()
        {
            // Arrange
            var invalidId = 0;

            // Act
            var result = await _controller.GetName(invalidId);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result.Result);
        }

        [Test]
        public async Task GetName_ReturnsOk_WithNameDto()
        {
            // Arrange
            var validId = 1;
            var name = new Names { Id = validId, Name = "test name" };
            var nameDto = new NameDto { Id = validId, Name = "test name" };

            _namesServiceMock.Setup(service => service.GetNameAsync(validId)).ReturnsAsync(name);
            _mapperMock.Setup(mapper => mapper.Map<NameDto>(name)).Returns(nameDto);

            // Act
            var result = await _controller.GetName(validId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(nameDto, okResult.Value);
        }

        [Test]
        public async Task GetNames_ReturnsOk_WithNamesDto()
        {
            // Arrange
            var names = new List<Names>
            {
                new Names { Id = 1, Name = "test name" },
                new Names { Id = 2, Name = "test name" }
            };

            var nameDtos = names.Select(n => new NameDto { Id = n.Id, Name = n.Name }).ToList();

            _namesServiceMock.Setup(service => service.AllAsync()).ReturnsAsync(names);
            _mapperMock.Setup(mapper => mapper.Map<NameDto>(It.IsAny<Names>())).Returns((Names src) => new NameDto { Id = src.Id, Name = src.Name });

            // Act
            var result = await _controller.GetNames();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            var returnedNames = okResult.Value as IEnumerable<NameDto>;
            Assert.AreEqual(nameDtos.Count, returnedNames.Count());
        }

        [Test]
        public async Task AddName_ReturnsBadRequest_WhenNameDtoIsInvalid()
        {
            // Arrange
            var invalidNameDto = new NameDto { Id = 1, Name = "" };

            // Act
            var result = await _controller.AddName(invalidNameDto);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task AddName_ReturnsCreatedAtAction_WhenNameDtoIsValid()
        {
            // Arrange
            var validNameDto = new NameDto { Id = 1, Name = "test name" };
            var validName = new Names { Id = 1, Name = "test name" };

            _mapperMock.Setup(mapper => mapper.Map<Names>(validNameDto)).Returns(validName);

            // Act
            var result = await _controller.AddName(validNameDto);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.AreEqual(nameof(_controller.GetName), createdAtActionResult.ActionName);
            Assert.AreEqual(validNameDto.Id, createdAtActionResult.RouteValues["id"]);
            Assert.AreEqual(validNameDto.Name, createdAtActionResult.Value);
        }

        [Test]
        public async Task UpdateName_ReturnsBadRequest_WhenNameDtoIsInvalid()
        {
            // Arrange
            var invalidNameDto = new NameDto { Id = 0, Name = "" };

            // Act
            var result = await _controller.UpdateName(invalidNameDto);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task UpdateName_ReturnsNoContent_WhenNameDtoIsValid()
        {
            // Arrange
            var validNameDto = new NameDto { Id = 1, Name = "test name" };
            var validName = new Names { Id = 1, Name = "test name" };

            _mapperMock.Setup(mapper => mapper.Map<Names>(validNameDto)).Returns(validName);

            // Act
            var result = await _controller.UpdateName(validNameDto);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteName_ReturnsBadRequest_WhenIdIsInvalid()
        {
            // Arrange
            var invalidId = 0;

            // Act
            var result = await _controller.DeleteName(invalidId);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task DeleteName_ReturnsNoContent_WhenIdIsValid()
        {
            // Arrange
            var validId = 1;

            // Act
            var result = await _controller.DeleteName(validId);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }
    }
}
