using System;
using System.Threading;
using System.Threading.Tasks;
using DVP.Tasks.Api.Application.Commands.Users;
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;
using DVP.Tasks.Infraestructure.Services;
using Moq;
using Xunit;

namespace DVP.Tasks.Api.Application.Commands.User.Tests
{
    public class DeleteUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IUserFinder> _mockUserFinder;
        private readonly Mock<IAzureActiveDirectoryService> _mockAadService;
        private readonly DeleteUserCommandHandler _handler;

        public DeleteUserCommandHandlerTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUserFinder = new Mock<IUserFinder>();
            _mockAadService = new Mock<IAzureActiveDirectoryService>();
            _handler = new DeleteUserCommandHandler(_mockUserRepository.Object, _mockUserFinder.Object, _mockAadService.Object);
        }

        [Fact]
        public async Task Handle_Should_Disable_User_When_User_Exists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new DeleteUserCommand { UserId = userId };

            var userToDelete = new Domain.AggregatesModel.UserAggregate.User(userId, "Test User", "test@example.com", "TestNickname", null);
            _mockUserFinder.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(userToDelete);
            _mockUserRepository.Setup(x => x.Disable(userToDelete)).Returns(Task.FromResult(true));
            _mockUserRepository.Setup(x => x.UnitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _mockAadService.Setup(x => x.disableAADUser(userId.ToString())).Returns(Task.FromResult(true));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True((bool)result);
            _mockUserFinder.Verify(x => x.FindByIdAsync(userId), Times.Once);
            _mockUserRepository.Verify(x => x.Disable(userToDelete), Times.Once);
            _mockUserRepository.Verify(x => x.UnitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mockAadService.Verify(x => x.disableAADUser(userId.ToString()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_User_Not_Found()
        {
            // Arrange
            var command = new DeleteUserCommand { UserId = Guid.NewGuid() };
            _mockUserFinder.Setup(x => x.FindByIdAsync(command.UserId)).ReturnsAsync((Domain.AggregatesModel.UserAggregate.User)null); // Simula que no se encontró el usuario

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Contains("User task not found", exception.Message);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_Disable_Fails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new DeleteUserCommand { UserId = userId };
            var userToDelete = new Domain.AggregatesModel.UserAggregate.User(userId, "Test User", "test@example.com", "TestNickname", null);

            _mockUserFinder.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(userToDelete);
            _mockUserRepository.Setup(x => x.Disable(userToDelete)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Contains("Database error", exception.Message);
        }
    }
}
