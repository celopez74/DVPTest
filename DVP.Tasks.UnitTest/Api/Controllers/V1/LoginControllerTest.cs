using DVP.Tasks.Api.Controllers.V1;
using DVP.Tasks.Api.SeedWork;
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;
using DVP.Tasks.Infraestructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DVP.Tasks.Api.Controllers.Tests
{
    public class LoginControllerTests
    {
        private readonly Mock<IUserFinder> _mockUserFinder;
        private readonly Mock<IAzureActiveDirectoryService> _mockAadService;
        private readonly LoginController _controller;

        public LoginControllerTests()
        {
            _mockUserFinder = new Mock<IUserFinder>();
            _mockAadService = new Mock<IAzureActiveDirectoryService>();
            _controller = new LoginController(_mockUserFinder.Object, _mockAadService.Object);
        }

        [Fact]
        public async Task Login_Should_Return_201_Created_When_Valid_User()
        {
            // Arrange
            var loginData = new UserLogin { Email = "test@example.com", Password = "Password123" };
            var user = new Domain.AggregatesModel.UserAggregate.User(Guid.NewGuid(),"User1","user1@example.com", "User1Nickname", null);

            _mockUserFinder.Setup(x => x.FindByEmailAsync(loginData.Email)).ReturnsAsync(user);
            _mockAadService.Setup(x => x.getTokenFromAAD(user.Id.ToString(), loginData.Password))
                           .ReturnsAsync(new AadTokenResponse { IsValidate = true, TokenResponse = new UserTokenResponse {access_token = "access-token"} });

            // Act
            var result = await _controller.Login(loginData);

            // Assert
            var createdResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, createdResult.StatusCode);             
        }

        [Fact]
        public async Task Login_Should_Return_400_BadRequest_When_User_Is_Disabled()
        {
            // Arrange
            var loginData = new UserLogin { Email = "test@example.com", Password = "Password123" };
            var user = new Domain.AggregatesModel.UserAggregate.User(Guid.NewGuid(),"User1","user1@example.com", "User1Nickname", null);
            user.IsEnabled = false;
            _mockUserFinder.Setup(x => x.FindByEmailAsync(loginData.Email)).ReturnsAsync(user);

            // Act
            var result = await _controller.Login(loginData);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var jsondata = JsonConvert.SerializeObject(badRequestResult.Value);
            Assert.Equal("User is Disable", JsonConvert.DeserializeObject<ResponseData>(jsondata).Message);
        }

        [Fact]
        public async Task Login_Should_Return_404_NotFound_When_User_Does_Not_Exist()
        {
            // Arrange
            var loginData = new UserLogin { Email = "test@example.com", Password = "Password123" };

            _mockUserFinder.Setup(x => x.FindByEmailAsync(loginData.Email)).ReturnsAsync((Domain.AggregatesModel.UserAggregate.User)null);

            // Act
            var result = await _controller.Login(loginData);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var jsondata = JsonConvert.SerializeObject(notFoundResult.Value);
            Assert.Equal($"User does not exist with id:test@example.com not found", JsonConvert.DeserializeObject<ResponseData>(jsondata).Message);
        }

        [Fact]
        public async Task Login_Should_Return_400_BadRequest_When_Invalid_Credentials()
        {
            // Arrange
            var loginData = new UserLogin { Email = "test@example.com", Password = "WrongPassword" };
            var user = new User(Guid.NewGuid(),"User1","user1@example.com", "User1Nickname", null);

            _mockUserFinder.Setup(x => x.FindByEmailAsync(loginData.Email)).ReturnsAsync(user);
            _mockAadService.Setup(x => x.getTokenFromAAD(user.Id.ToString(), loginData.Password))
                           .ReturnsAsync(new AadTokenResponse { IsValidate = false, TokenErrorResponse = new UserTokenErrorResponse { error_description = "AADSTS50126: Invalid username or password" } });

            // Act
            var result = await _controller.Login(loginData);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var jsondata = JsonConvert.SerializeObject(notFoundResult.Value);
            Assert.Equal("Error validating credentials due to invalid username or password", JsonConvert.DeserializeObject<ResponseData>(jsondata).Message);
        }

        [Fact]
        public async Task Login_Should_Return_500_InternalServerError_On_Exception()
        {
            // Arrange
            var loginData = new UserLogin { Email = "test@example.com", Password = "Password123" };
            var user = new User(Guid.NewGuid(),"User1","user1@example.com", "User1Nickname", null);

            _mockUserFinder.Setup(x => x.FindByEmailAsync(loginData.Email)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Login(loginData);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("Unexpected error", objectResult.Value);
        }
    }
}
