using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Net;
using Newtonsoft.Json;
using xv_dotnet_demo.Controllers;
using xv_dotnet_demo.Dtos;
using xv_dotnet_demo_v2_services;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace xv_dotnet_demo_tests
{
    public class ApiRickAndMortyControllerTests
    {
        private Mock<IRickAndMortyService> _rickAndMortyServiceMock;
        private ApiRickAndMortyController _controller;

        [SetUp]
        public void Setup()
        {
            _rickAndMortyServiceMock = new Mock<IRickAndMortyService>();
            _controller = new ApiRickAndMortyController(_rickAndMortyServiceMock.Object);
        }

        [Test]
        public async Task GetCharacter_ReturnsBadRequest_WhenIdIsLessThanOrEqualToZero()
        {
            // Arrange
            var invalidId = 0;

            // Act
            var result = await _controller.GetCharacter(invalidId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("Id should be greater than 0", badRequestResult.Value);
        }

        [Test]
        public async Task GetCharacter_ReturnsOkResultWithCharacterDto_WhenIdIsValid()
        {
            // Arrange
            var validId = 1;
            var characterJson = JsonSerializer.Serialize(new CharacterDto { id = validId, name = "Rick Sanchez" });
            _rickAndMortyServiceMock.Setup(x => x.GetCharacterAsync(validId)).ReturnsAsync(characterJson);

            // Act
            var result = await _controller.GetCharacter(validId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var characterDto = okResult.Value as CharacterDto;
            Assert.IsNotNull(characterDto);
            Assert.AreEqual(validId, characterDto.id);
            Assert.AreEqual("Rick Sanchez", characterDto.name);
        }

        [Test]
        public async Task GetCharacter_ReturnsBadGateway_WhenHttpRequestExceptionIsThrown()
        {
            // Arrange
            var validId = 1;
            _rickAndMortyServiceMock.Setup(x => x.GetCharacterAsync(validId))
                                    .ThrowsAsync(new HttpRequestException("Service unavailable"));

            // Act
            var result = await _controller.GetCharacter(validId);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult.Value);
            Assert.AreEqual((int)HttpStatusCode.BadGateway, objectResult.StatusCode);
        }

        [Test]
        public async Task GetCharacter_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var validId = 1;
            _rickAndMortyServiceMock.Setup(x => x.GetCharacterAsync(validId))
                                    .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetCharacter(validId);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult.Value);
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
        }
    }
}
