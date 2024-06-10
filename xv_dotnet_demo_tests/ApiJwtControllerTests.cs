using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using xv_dotnet_demo.Controllers;
using xv_dotnet_demo.Dtos;
using xv_dotnet_demo_v2_domain.Authorization;
using xv_dotnet_demo_v2_domain.Exceptions;
using xv_dotnet_demo_v2_services;

namespace xv_dotnet_demo_tests
{
    public class ApiJwtControllerTests
    {
        private Mock<ILogger<ApiJwtController>> _loggerMock;
        private Mock<IAuthorizationService> _authorizationServiceMock;
        private Mock<IMapper> _mapperMock;
        private ApiJwtController _controller;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<ApiJwtController>>();
            _authorizationServiceMock = new Mock<IAuthorizationService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new ApiJwtController(_loggerMock.Object, _authorizationServiceMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task BuildToken_ReturnsOkResultWithTokenDto()
        {
            // Arrange
            var token = new Token();
            var tokenDto = new TokenDto();
            _authorizationServiceMock.Setup(x => x.BuildToken(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(token);
            _mapperMock.Setup(x => x.Map<TokenDto>(token)).Returns(tokenDto);

            // Act
            var result = await _controller.buildToken();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(tokenDto, okResult.Value);
        }

        [Test]
        public async Task ValidateToken_ReturnsOkResultWithIssuerDataDto_WhenTokenIsValid()
        {
            // Arrange
            var token = "valid_token";
            var issuerData = new IssuerData { Issuer = "issuer" };
            var issuerDataDto = new IssuerDataDto();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.Request.Headers["Authorization"] = $"Bearer {token}";

            _authorizationServiceMock.Setup(x => x.ValidateToken(token, It.IsAny<string>())).ReturnsAsync(issuerData);
            _mapperMock.Setup(x => x.Map<IssuerDataDto>(issuerData)).Returns(issuerDataDto);

            // Act
            var result = await _controller.validateToken();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(issuerDataDto, okResult.Value);
        }

        [Test]
        public async Task ValidateToken_ReturnsUnauthorized_WhenValidateTokenExceptionIsThrown()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            _controller.Request.Headers["Authorization"] = $"Bearer invalid_token";

            _authorizationServiceMock.Setup(x => x.ValidateToken(It.IsAny<string>(), It.IsAny<string>()))
                                     .ThrowsAsync(new ValidateTokenException("Invalid token"));

            // Act
            var result = await _controller.validateToken();

            // Assert
            Assert.IsInstanceOf<StatusCodeResult>(result);
            var objectResult = result as StatusCodeResult;
            Assert.AreEqual(401, objectResult.StatusCode);
        }
    }
}
