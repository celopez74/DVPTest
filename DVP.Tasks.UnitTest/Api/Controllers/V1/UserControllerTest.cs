using DVP.Tasks.Api.Application.Commands.Users;
using DVP.Tasks.Api.Application.Queries.Users;
using DVP.Tasks.Api.Controllers.V1;
using DVP.Tasks.Api.SeedWork;
using DVP.Tasks.Domain.AggregatesModel.RoleAggregate;
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;
using DVP.Tasks.Domain.Exception;
using DVP.Tasks.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DVP.Tasks.Api.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UserController _userController;

        public UserControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _userController = new UserController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetUserById_ReturnsOkResult_WhenUserFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new UserDto { Name = "Test User", Email = "test@example.com" }; 
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetUserQuery>(), default))
                .ReturnsAsync(user);

            // Act
            var result = await _userController.GetUserById(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);            
        }

        [Fact]
        public async Task GetUserById_ReturnsNotFound_WhenUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetUserQuery>(), default))
                .ThrowsAsync(new EntityNotFoundException(userId.ToString(), "User not found"));

            // Act
            var result = await _userController.GetUserById(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var jsondata = JsonConvert.SerializeObject(notFoundResult.Value);
            Assert.Equal($"User not found with id:{userId} not found", JsonConvert.DeserializeObject<ResponseData>(jsondata).Message);          
        }

        [Fact]
        public async Task GetAllUsers_ReturnsOkResult_WhenUsersFound()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var userList = new List<Domain.AggregatesModel.UserAggregate.User>
            {
                new Domain.AggregatesModel.UserAggregate.User(Guid.NewGuid(),"User1","user1@example.com", "User1Nickname", null),
                new Domain.AggregatesModel.UserAggregate.User(Guid.NewGuid(),"User2","user2@example.com", "User2Nickname", null),
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetUserListQuery>(), default))
                .ReturnsAsync(userList);

            // Act
            var result = await _userController.GetAllUsers(pageNumber, pageSize);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = okResult?.Value as ResponseData;
            var userlistResult = JsonConvert.DeserializeObject<List<UserDto>>(responseData.Data.ToString());
            Assert.Equal(userList.Count(), userlistResult.Count());
            Assert.Equal(userList[0].Name, userlistResult[0].Name);     
            Assert.Equal(userList[0].Email, userlistResult[0].Email);
            Assert.Equal(userList[0].Nickname, userlistResult[0].Nickname); 
            Assert.Equal(userList[1].Name, userlistResult[1].Name);   
            Assert.Equal(userList[1].Email, userlistResult[1].Email);
            Assert.Equal(userList[1].Nickname, userlistResult[1].Nickname); 

        }

        [Fact]
        public async Task CreateUser_ReturnsCreatedResult_WhenUserCreated()
        {
            // Arrange
            var command = new CreateUserCommand { Name = "New User", Email = "newuser@example.com" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateUserCommand>(), default))
                .ReturnsAsync(command);

            // Act
            var result = await _userController.CreateUser(command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = okResult?.Value as ResponseData;
            var userlistResult = JsonConvert.DeserializeObject<UserDto>(responseData.Data.ToString());
            Assert.Equal(command.Name, userlistResult.Name);
            Assert.Equal(command.Email, userlistResult.Email);
        }

        [Fact]
        public async Task AddUserToRole_ReturnsCreatedResult_WhenRoleAdded()
        {
            // Arrange
            var command = new AddUserToRoleCommand { UserId = Guid.NewGuid(), RoleId = RolesEnum.Suppervisor };
            _mediatorMock.Setup(m => m.Send(It.IsAny<AddUserToRoleCommand>(), default))
                .ReturnsAsync(command);

            // Act
            var result = await _userController.AddUserToRole(command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = okResult?.Value as ResponseData;
            var userRoleResult = JsonConvert.DeserializeObject<UserRole>(responseData.Data.ToString());
            Assert.Equal(command.UserId, userRoleResult.UserId);
            Assert.Equal((int)command.RoleId, userRoleResult.RoleId);
        }

        [Fact]
        public async Task UpdateUser_ReturnsOkResult_WhenUserUpdated()
        {
            // Arrange
            var command = new UpdateUserCommand { Id = Guid.NewGuid(), Name = "Updated User" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), default))
                .ReturnsAsync(command);

            // Act
            var result = await _userController.UpdateUser(command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task DeleteUserTask_ReturnsOkResult_WhenUserDeleted()
        {
            // Arrange
            var command = new DeleteUserCommand { UserId = Guid.NewGuid() };
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), default))
                .ReturnsAsync(command);

            // Act
            var result = await _userController.DeleteUserTask(command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsBadRequest_WhenExceptionThrown()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetUserListQuery>(), default))
                .ThrowsAsync(new BadRequestException("Bad request"));

            // Act
            var result = await _userController.GetAllUsers(pageNumber, pageSize);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var responseData = badRequestResult?.Value as ResponseData;
            Assert.Equal("Bad request", responseData.Message);
        }
    }
}
