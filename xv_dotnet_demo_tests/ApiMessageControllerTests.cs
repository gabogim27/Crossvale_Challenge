using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using xv_dotnet_demo.Controllers;
using xv_dotnet_demo.Dtos;
using xv_dotnet_demo_v2_domain.Entities;
using xv_dotnet_demo_v2_services;

namespace xv_dotnet_demo_tests
{
    public class ApiMessageControllerTests
    {
        private Mock<IMessageService> _messageServiceMock;
        private Mock<IMapper> _mapperMock;
        private ApiMessageController _controller;

        [SetUp]
        public void Setup()
        {
            _messageServiceMock = new Mock<IMessageService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new ApiMessageController(_messageServiceMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetMessage_ReturnsBadRequest_WhenIdIsInvalid()
        {
            // Arrange
            var invalidId = 0;

            // Act
            var result = await _controller.GetMessage(invalidId);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result.Result);
        }

        [Test]
        public async Task GetMessage_ReturnsOk_WithMessageDto()
        {
            // Arrange
            var validId = 1;
            var message = new Message { id = validId, message = "Hello World" };
            var messageDto = new MessageDto { id = validId, message = "Hello World" };

            _messageServiceMock.Setup(service => service.GetMessageAsync(validId)).ReturnsAsync(message);
            _mapperMock.Setup(mapper => mapper.Map<MessageDto>(message)).Returns(messageDto);

            // Act
            var result = await _controller.GetMessage(validId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(messageDto, okResult.Value);
        }

        [Test]
        public async Task GetMessages_ReturnsOk_WithMessagesDto()
        {
            // Arrange
            var messages = new List<Message>
            {
                new Message { id = 1, message = "Hello" },
                new Message { id = 2, message = "World" }
            };

            var messageDtos = messages.Select(m => new MessageDto { id = m.id, message = m.message }).ToList();

            _messageServiceMock.Setup(service => service.GetMessagesAsync()).ReturnsAsync(messages);
            _mapperMock.Setup(mapper => mapper.Map<MessageDto>(It.IsAny<Message>())).Returns((Message src) => new MessageDto { id = src.id, message = src.message });

            // Act
            var result = await _controller.GetMessages();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            var returnedMessages = okResult.Value as IEnumerable<MessageDto>;
            Assert.AreEqual(messageDtos.Count, returnedMessages.Count());
        }

        [Test]
        public async Task AddMessage_ReturnsBadRequest_WhenMessageIsInvalid()
        {
            // Arrange
            var invalidMessage = new MessageDto { id = 0, message = "" };

            // Act
            var result = await _controller.AddMessage(invalidMessage);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task AddMessage_ReturnsCreatedAtAction_WhenMessageIsValid()
        {
            // Arrange
            var validMessageDto = new MessageDto { id = 1, message = "Hello World" };
            var validMessage = new Message { id = 1, message = "Hello World" };

            _mapperMock.Setup(mapper => mapper.Map<Message>(validMessageDto)).Returns(validMessage);

            // Act
            var result = await _controller.AddMessage(validMessageDto);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.AreEqual(nameof(_controller.GetMessage), createdAtActionResult.ActionName);
            Assert.AreEqual(validMessageDto.id, createdAtActionResult.RouteValues["id"]);
            Assert.AreEqual(validMessageDto, createdAtActionResult.Value);
        }

        [Test]
        public async Task UpdateMessage_ReturnsBadRequest_WhenMessageIsInvalid()
        {
            // Arrange
            var invalidMessageDto = new MessageDto { id = 0, message = "" };

            // Act
            var result = await _controller.UpdateMessage(invalidMessageDto);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task UpdateMessage_ReturnsNoContent_WhenMessageIsValid()
        {
            // Arrange
            var validMessageDto = new MessageDto { id = 1, message = "Hello World" };
            var validMessage = new Message { id = 1, message = "Hello World" };

            _mapperMock.Setup(mapper => mapper.Map<Message>(validMessageDto)).Returns(validMessage);

            // Act
            var result = await _controller.UpdateMessage(validMessageDto);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteMessage_ReturnsBadRequest_WhenIdIsInvalid()
        {
            // Arrange
            var invalidId = 0;

            // Act
            var result = await _controller.DeleteMessage(invalidId);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task DeleteMessage_ReturnsNoContent_WhenIdIsValid()
        {
            // Arrange
            var validId = 1;

            // Act
            var result = await _controller.DeleteMessage(validId);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }
    }
}
